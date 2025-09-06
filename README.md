# LinkLynx

LinkLynx is a lightweight development framework designed to drastically speed up and simplify development for most small to medium Crestron programs for 4-Series Processors Only. (Pre Release Early Alpha!!)

## Table of Contents
- [Download](#download)
- [Requirements](#requirements)
- [Installation](#installation)
- [Usage Example](#usage-example)
- [Usage](docs/Usage.md)
- [API Reference](docs/API.md)
- [Features](#features)
- [License](#license)


## Download
Get the latest DLL from the [Releases page](https://github.com/MithrilComputer/LinkLynx/releases/latest).


## Requirements
These are the requirements for using this Framework.
- .NET Framework: 4.7.2
- Crestron NuGet Packages:
	- Crestron.SimplSharp.SDK.Library.2.21.133 or later
	- Crestron.SimplSharp.SDK.Program.2.21.133 or later
	- Crestron.SimplSharp.SDK.ProgramLibrary.2.21.133 or later
- IDE: Visual Studio 2019 or later
- Target Platform: 4-Series processors (SIMPL# Pro projects)

## Usage Example

```csharp
using System;

using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;

using LinkLynx.Core.Utility.Helpers;
using LinkLynx.Core.Utility.Signals;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.Utility.Signals.Attributes;

// This is a single logic page example.

// Program.cs (SIMPL# Pro Entry Point)
public class ControlSystem : CrestronControlSystem
{
    public ControlSystem() : base() { }

    public override void InitializeSystem()
    {
        LinkLynx.Boot.Initialize();                       // 1) scan & register

        var tp = new Tsw1060(0x03, this);                 // 2) your panel
        tp.Register();                                      

        LinkLynx.Boot.RegisterPanel(tp);                  // 3) attach logic
    }

    void _ControllerProgramEventHandler(eProgramStatusEventType programStatusEventType)
    {
        switch (programStatusEventType)
        {
            case (eProgramStatusEventType.Stopping):
                    
                SystemsLocator.LinkLynxInstance.Cleanup(); // Clean up the Framework

                break;
        }
    }
}

// RelayPageInfo.cs
internal static class RelayPageInfo
{
    internal const ushort PageId = 1;

    internal const uint RelayID = 1;

    /// <summary>
    /// The collection of digital joins used in the page.
    /// </summary>
    [SigType(eSigType.Bool)] // Marks the enum as a digital type
    public enum DigitalJoins : uint
    {
        // On Button Joins
        OnButtonPress = 10,
        OnButtonEnable = 11,

        OffButtonPress = 12,
        OffButtonEnable = 13,
    }
}

// RelayPage.cs
[Page(RelayPageInfo.PageId)]
internal class RelayPage : PageLogicBase
{
    public RelayPage(BasicTriList device) : base(device) { }

    private bool RelayState;

    /// <summary>
    /// Sets the default state of the page.
    /// </summary>
    public override void SetDefaults()
    {
        UpdateButtonStates();
    }

    /// <summary>
    /// Initializes the page logic, runs once.
    /// </summary>
    public override void Initialize()
    {
        RelayState = SystemsLocator.ControlSystemInstance.RelayPorts[RelayPageInfo.RelayID].State;

        UpdateButtonStates();

        SystemsLocator.ControlSystemInstance.RelayPorts[RelayPageInfo.RelayID].StateChange += RelayStateChange;
    }

    /// <summary>
    /// Handles the state change event for a specified relay.
    /// </summary>
    /// <remarks>This method updates the internal state of the relay and adjusts the button states
    /// accordingly  if the relay matches the configured relay ID in <see cref="RelayPageInfo"/>.</remarks>
    /// <param name="relay">The <see cref="Relay"/> instance whose state has changed.</param>
    /// <param name="args">The event arguments containing additional information about the state change.</param>
    private void RelayStateChange(Relay relay, RelayEventArgs args)
    {
        if(relay.ID == RelayPageInfo.RelayID)
        {
            RelayState = SystemsLocator.ControlSystemInstance.RelayPorts[RelayPageInfo.RelayID].State;

            UpdateButtonStates();
        }
    }

    /// <summary>
    /// Updates the enabled states of the "On" and "Off" buttons based on the current relay state.
    /// </summary>
    /// <remarks>This method sets the logic joins for the "On" and "Off" buttons to reflect the
    /// current state of the relay. The "On" button is enabled when the relay is off, and the "Off" button is
    /// enabled when the relay is on.</remarks>
    private void UpdateButtonStates()
    {
        SignalHelper.SetLogicJoin(assignedPanel, RelayPageInfo.DigitalJoins.OnButtonEnable, !RelayState);
        SignalHelper.SetLogicJoin(assignedPanel, RelayPageInfo.DigitalJoins.OffButtonEnable, RelayState);
    }

    /// <summary>
    /// Handles the "Turn On" button press event.
    /// </summary>
    /// <remarks>This method is triggered when the associated "Turn On" button is pressed. It performs
    /// an action  based on the signal's rising edge state. Ensure that the signal state is properly managed to 
    /// avoid unintended behavior.</remarks>
    /// <param name="args">The event arguments containing the signal state information.</param>
    [Join(RelayPageInfo.DigitalJoins.OnButtonPress)]
    public void OnTurnOnPress(SigEventArgs args)
    {
        if (SignalHelper.IsRisingEdge(args))
        {
            SystemsLocator.ControlSystemInstance.RelayPorts[RelayPageInfo.RelayID].Close();
        }
    }

    /// <summary>
    /// Handles the event triggered when the "Off" button is pressed.
    /// </summary>
    /// <remarks>This method is invoked when the associated "Off" button press event occurs.  It
    /// performs an action only if the signal indicates a rising edge.</remarks>
    /// <param name="args">The event arguments containing the signal state information.</param>
    [Join(RelayPageInfo.DigitalJoins.OffButtonPress)]
    public void OnTurnOffPress(SigEventArgs args)
    {
        if (SignalHelper.IsRisingEdge(args))
        {
            SystemsLocator.ControlSystemInstance.RelayPorts[RelayPageInfo.RelayID].Open();
        }
    }
}

```


## Installation
1. Add Required Crestron NuGet packages.
2. Download the latest `LinkLynx.dll` from [Releases](#download).
3. In your SIMPL# Pro project, **Add Reference** → **Browse** to the DLL.

## Features
- **One-call startup**
  - `Initialize()` scans assemblies, registers pages, and auto-wires joins.
  - `RegisterPanel(panel)` creates a per-panel logic group containing all the logic required to run the panel.
  - `SetPanelToDefaultState(panel)` runs each page’s `SetDefaults()` for default UI state.

- **Attribute-driven pages**
  - `[Page(id)]` marks a class as a page; discovered via reflection.
  - Pages are constructed per panel through a `PageFactory`.

- **Automatic join binding**
  - Tag handlers with `[Join(SomeJoinEnum)]` and they’re auto-registered.
  - Join type (Digital/Analog/Serial) is inferred from the 
  [SigType(eSigType)] Attribute that is attached to the Enum, this ensures complete type safety.

- **Signal routing pipeline**
  - `SignalProcessor` → `JoinInstanceRouter` → page action.
  - Reverse lookup maps `(join, type)` → **page ID** for correct dispatch.

- **Per-panel logic isolation**
  - Each panel gets its own `PanelLogicGroup` with a private page pool.
  - No cross-talk between panels; safe for multi-panel systems.

- **Dispatcher per signal type**
  - Separate digital/analog/serial dispatchers for clean separation.
  - Fast lookup: add, check, and invoke by join number.

- **IPID management (panels)**
  - Debug helpers show used/available IPIDs.

- **Developer helpers**
  - `SignalHelper.SetLogicJoin<T>(BasicTriList panel, Enum join, T value)` for quick panel writes.
  - `SignalHelper.IsRisingEdge(args)` utility for simple button edge detection.
  - Consistent console logging for ops/debug.

- **Debugging helpers**
  - `ConsoleLogger.Log(String Message)` For logging to the Crestron console more simply and with future compatibility in mind.

- **Clean shutdown**
  - `Cleanup()` clears pools, registries, and dispatchers in order to prevent leaks.

## License

<br>

LinkLynx is licensed under the Apache License 2.0.  
You are free to use, modify, and distribute this software, provided that you include the license text and copyright notice.  
See the [LICENSE](LICENSE) file for details.

<br>
