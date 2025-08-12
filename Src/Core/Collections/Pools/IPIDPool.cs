using Crestron.SimplSharp;
using Crestron.SimplSharpPro.DeviceSupport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkLynx.Core.Collections
{
    internal static class IPIDPool
    {
        /// <summary>
        /// The reserved minimum range for the IPIDs that are only used by direct calls to add or remove.
        /// </summary>
        private const uint reservedIPIDMin = 3;

        /// <summary>
        /// The reserved maximum range for the IPIDs that are only used by direct calls to add or remove.
        /// </summary>
        private const uint reservedIPIDMax = 83;

        /// <summary>
        /// The IPID that is used to assign a new dynamic IPID from external requests, cannot be used by any device as a identifier. Any web panel with this IPID will be automatically assigned a new dynamic IPID from the pool.
        /// </summary>
        private const uint ipidAutoAssignID = 84;

        /// <summary>
        /// The IPID that is used to assign a new dynamic IPID from external requests, cannot be used by any device as a identifier. Any web panel with this IPID will be automatically assigned a new dynamic IPID from the pool.
        /// </summary>
        public static uint IPIDAutoAssignValue => ipidAutoAssignID;

        /// <summary>
        /// The minimum value for dynamic IPIDs that can be assigned to devices.
        /// </summary>
        private const uint dynamicIPIDValuesMinimum = 85;

        /// <summary>
        /// The maximum value for dynamic IPIDs that can be assigned to devices.
        /// </summary>
        private const uint dynamicIPIDValuesMaximum = 240;

        /// <summary>
        /// The pool of available IPIDs for devices.
        /// </summary>
        private static Queue<uint> availableDynamicIPIDs = new Queue<uint>();

        /// <summary>
        /// The set of used IPIDs to prevent duplicates.
        /// </summary>
        private static HashSet<uint> usedIPIDS = new HashSet<uint>();

        /// <summary>
        /// a registry of all panels that are currently using an IPID.
        /// </summary>
        private static Dictionary<uint, BasicTriList> panelDictionary = new Dictionary<uint, BasicTriList>();

        /// <summary>
        /// The constructor for the IPIDPool class.
        /// </summary>
        static IPIDPool()
        {
            // Fill queue with valid IPIDs
            for (uint i = dynamicIPIDValuesMinimum; i <= dynamicIPIDValuesMaximum; i++)
            {
                availableDynamicIPIDs.Enqueue(i);
            }

            CrestronConsole.PrintLine("[IPIDPool] Log: initialized.");
        }

        /// <summary>
        /// Assigns the next available IPID to a device.
        /// </summary>
        /// <remarks>The IPID will be between the values of 85 and 240</remarks>
        /// <param name="device">The device to assign a new IPID to</param>
        /// <returns>The IPID that was assigned to the device</returns>
        internal static uint AssignDynamicDeviceIPID(BasicTriList device)
        {
            if(device == null)
            {
                CrestronConsole.PrintLine("[IPIDPool] Warning: Device is null, cannot assign IPID.");
                return 0;
            }

            if (availableDynamicIPIDs.Count == 0)
            {
                throw new Exception("[IPIDPool] Error: No available IPIDs in the pool.");
            }

            uint ipid = availableDynamicIPIDs.Dequeue();

            panelDictionary.Add(ipid, device);
            usedIPIDS.Add(ipid);

            CrestronConsole.PrintLine($"[IPIDPool] Log: Assigned device '{device.Name}' to IPID 0x{ipid:X2}");

            return ipid;
        }

        /// <summary>
        /// Adds a new IPID to the pool if it is not already in use.
        /// </summary>
        /// <remarks>Keep the IPID used to between the reserved 3 - 83 devices</remarks>
        /// <param name="ipid"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        internal static bool ReserveIPIDToDevice(uint ipid, BasicTriList device)
        {
            if (usedIPIDS.Contains(ipid))
            {
                CrestronConsole.PrintLine($"[IPIDPool] Warning: IPID '0x{ipid:X2}' already exists in global IPIDs.");
                return false; // IPID already in use
            }

            if (ipid < reservedIPIDMin || ipid > reservedIPIDMax)
            {
                CrestronConsole.PrintLine($"[IPIDPool] Warning: IPID '0x{ipid:X2}' is outside reserved range ({reservedIPIDMin}–{reservedIPIDMax}).");
                return false;
            }

            CrestronConsole.PrintLine("[IPIDPool] Log: Adding device to IPID pool");
            
            panelDictionary.Add(ipid, device);

            usedIPIDS.Add(ipid);
            return true;
        }

        /// <summary>
        /// Removes an IPID from the pool and adds it back to the available pool.
        /// </summary>
        /// <param name="ipid">The ipid of the device you would like to remove from the pool.</param>
        /// <returns>If the operation was successful</returns>
        internal static bool RemoveIPID(uint ipid)
        {
            if (!usedIPIDS.Contains(ipid))
            {
                CrestronConsole.PrintLine($"[IPIDPool] Warning: IPID '0x{ipid:X2}' not found in global IPIDs.");
                return false;
            }

            if (ipid < dynamicIPIDValuesMinimum || ipid > dynamicIPIDValuesMaximum)
            {
                CrestronConsole.PrintLine($"[IPIDPool] Warning: Cannot release reserved IPID: 0x{ipid:X2} back to the dynamic pool.");
                return false;
            }

            CrestronConsole.PrintLine("[IPIDPool] Log: Removing device from IPID pool");

            panelDictionary.Remove(ipid);

            usedIPIDS.Remove(ipid);
            availableDynamicIPIDs.Enqueue(ipid);
            return true;
        }

        /// <summary>
        /// Checks if an IPID is taken from the pool.
        /// </summary>
        /// <param name="ipid">The device IPID to check if exists in the pool.</param>
        /// <returns>If the device was found in the pool</returns>
        internal static bool CheckIfIPIDTaken(uint ipid)
        {
            return usedIPIDS.Contains(ipid);
        }

        /// <summary>
        /// Gets a device from the IPID pool based on the IPID.
        /// </summary>
        /// <param name="ipid">The ipid of the device you want to get from the pool.</param>
        /// <returns>The device with the associated IPID</returns>
        internal static BasicTriList GetDeviceFromIPID(uint ipid)
        {
            if (panelDictionary.TryGetValue(ipid, out var device))
                return device;

            CrestronConsole.PrintLine($"[IPIDPool] Warning: IPID '0x{ipid:X2}' not found in the pool.");
            return null; // IPID not found
        }

        /// <summary>
        /// A debug method to print the current status of the IPID pool.
        /// </summary>
        internal static void DebugStatus()
        {
            CrestronConsole.PrintLine($"[IPIDPool] Debug: Used IPIDs: {usedIPIDS.Count}, Available: {availableDynamicIPIDs.Count}");
            if (usedIPIDS.Count > 0)
                CrestronConsole.PrintLine("[IPIDPool] Debug:  Active IPIDs: " + string.Join(", ", usedIPIDS.Select(x => $"0x{x:X2}")));
        }

        /// <summary>
        /// Clears the IPID pool, should only be called on system shutdown
        /// </summary>
        internal static void Clear()
        {
            availableDynamicIPIDs.Clear();
            usedIPIDS.Clear();
            panelDictionary.Clear();
        }
    }
}
