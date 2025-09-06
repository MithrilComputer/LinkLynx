using Crestron.SimplSharp;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Utility.Debugging.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkLynx.Core.Collections.Pools
{
    /// <summary>
    /// The pool of all the connected panels.
    /// </summary>
    public sealed class PanelPool
    {
        /// <summary>
        /// The singleton instance of the class.
        /// </summary>
        private static readonly PanelPool instance = new PanelPool();

        /// <summary>
        /// The singleton instance of the class.
        /// </summary>
        public static PanelPool Instance => instance;

        /// <summary>
        /// Class constructor
        /// </summary>
        public PanelPool()
        {            // Fill queue with valid IPIDs
            for (uint i = dynamicIPIDValuesMinimum; i <= dynamicIPIDValuesMaximum; i++)
            {
                availableDynamicIPIDs.Enqueue(i);
            }
        }

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
        public uint IPIDAutoAssignValue => ipidAutoAssignID;

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
        private readonly Queue<uint> availableDynamicIPIDs = new Queue<uint>();

        /// <summary>
        /// The set of used IPIDs to prevent duplicates.
        /// </summary>
        private readonly HashSet<uint> usedIPIDS = new HashSet<uint>();

        /// <summary>
        /// a registry of all panels that are currently using an IPID.
        /// </summary>
        private readonly Dictionary<uint, BasicTriList> panelDictionary = new Dictionary<uint, BasicTriList>();

        /// <summary>
        /// Assigns the next available IPID to a device.
        /// </summary>
        /// <remarks>The IPID will be between the values of 85 and 240</remarks>
        /// <param name="device">The device to assign a new IPID to</param>
        /// <returns>The IPID that was assigned to the device</returns>
        internal uint AssignDynamicDeviceToIpId(BasicTriList device)
        {
            if(device == null)
            {
                ConsoleLogger.Log("[PanelPool] Warning: Device is null, cannot assign IPID.");
                return 0;
            }

            if (availableDynamicIPIDs.Count == 0)
            {
                throw new Exception("[PanelPool] Error: No available IPIDs in the pool.");
            }

            uint ipid = availableDynamicIPIDs.Dequeue();

            panelDictionary.Add(ipid, device);
            usedIPIDS.Add(ipid);

            ConsoleLogger.Log($"[PanelPool] Log: Assigned device '{device.Name}' to IPID 0x{ipid:X2}");

            return ipid;
        }

        /// <summary>
        /// Adds a new panel's IPID to the pool if it is not already in use.
        /// </summary>
        /// <remarks>Keep the IPID used to between the reserved 3 - 83 panels</remarks>
        /// <param name="ipid"></param>
        /// <param name="panel"></param>
        /// <returns></returns>
        internal bool ReserveIPIDToPanel(uint ipid, BasicTriList panel)
        {
            if (usedIPIDS.Contains(ipid))
            {
                ConsoleLogger.Log($"[PanelPool] Warning: IPID '0x{ipid:X2}' already exists in global IPIDs.");
                return false; // IPID already in use
            }

            if (ipid < reservedIPIDMin || ipid > reservedIPIDMax)
            {
                ConsoleLogger.Log($"[PanelPool] Warning: IPID '0x{ipid:X2}' is outside reserved range ({reservedIPIDMin}–{reservedIPIDMax}).");
                return false;
            }

            ConsoleLogger.Log("[PanelPool] Log: Adding device to IPID pool");
            
            panelDictionary.Add(ipid, panel);

            usedIPIDS.Add(ipid);
            return true;
        }

        /// <summary>
        /// Removes an IPID from the pool and adds it back to the available pool.
        /// </summary>
        /// <param name="ipid">The ipid of the device you would like to remove from the pool.</param>
        /// <returns>If the operation was successful</returns>
        internal bool RemoveIPID(uint ipid)
        {
            if (!usedIPIDS.Contains(ipid))
            {
                ConsoleLogger.Log($"[PanelPool] Warning: IPID '0x{ipid:X2}' not found in global IPIDs.");
                return false;
            }

            if (ipid < dynamicIPIDValuesMinimum || ipid > dynamicIPIDValuesMaximum)
            {
                ConsoleLogger.Log($"[PanelPool] Warning: Cannot release reserved IPID: 0x{ipid:X2} back to the dynamic pool.");
                return false;
            }

            ConsoleLogger.Log("[PanelPool] Log: Removing device from IPID pool");

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
        internal bool CheckIfIPIDTaken(uint ipid)
        {
            return usedIPIDS.Contains(ipid);
        }

        /// <summary>
        /// Gets a device from the IPID pool based on the IPID.
        /// </summary>
        /// <param name="ipid">The ipid of the device you want to get from the pool.</param>
        /// <returns>The device with the associated IPID</returns>
        public BasicTriList GetDeviceFromIPID(uint ipid)
        {
            if (panelDictionary.TryGetValue(ipid, out var device))
                return device;

            ConsoleLogger.Log($"[PanelPool] Warning: IPID '0x{ipid:X2}' not found in the pool.");
            return null; // IPID not found
        }

        /// <summary>
        /// A debug method to print the current status of the IPID pool.
        /// </summary>
        internal void DebugStatus()
        {
            ConsoleLogger.Log($"[PanelPool] Debug: Used IPIDs: {usedIPIDS.Count}, Available: {availableDynamicIPIDs.Count}");
            if (usedIPIDS.Count > 0)
                ConsoleLogger.Log("[PanelPool] Debug:  Active IPIDs: " + string.Join(", ", usedIPIDS.Select(x => $"0x{x:X2}")));
        }

        /// <summary>
        /// Clears the IPID pool, should only be called on system shutdown
        /// </summary>
        internal void Clear()
        {
            availableDynamicIPIDs.Clear();
            usedIPIDS.Clear();
            panelDictionary.Clear();
        }
    }
}
