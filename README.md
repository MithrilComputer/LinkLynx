#LinkLynx
LinkLynx is a lightweight development framework meant to drastically speed up and simplify development for most small to medium Crestron programs.

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
These are the requirements for using this library.
- .NET Framework: 4.7.2
- Crestron NuGet Packages:
	- Crestron.SimplSharp.SDK.Library.2.21.133 or later
	- Crestron.SimplSharp.SDK.Program.2.21.133 or later
	- Crestron.SimplSharp.SDK.ProgramLibrary.2.21.133 or later
- IDE: Visual Studio 2019 or later
- Target Platform: 4-Series processors (SIMPL# Pro projects)

## Usage Example

```csharp
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;

using LinkLynx.Core.Utility.Helpers;
using LinkLynx.Core.Utility.Signals;
using LinkLynx.Core.Logic.Pages;
using System;

// This is a single logic page example.

[Page(MainPageInfo.PageID)]
internal class MainPageLogic : PageLogicBase
{
    public MainPageLogic(BasicTriList device) : base(device) { }

    bool isTimeDisplayed;

    ushort gaugeValue;

    string someText;

    Random random;

    // Initializes the logic for the page.
    public override void Initialize()
    {
        if(random == null)
        {
            random = new Random();
        }

        someText = "Want A Random Number?";
        isTimeDisplayed = false;
        gaugeValue = 0;

        SignalHelper.SetDigitalJoin(assignedPanel, (uint)MainPageInfo.DigitalJoins.WeatherWidgetVisibility, false);

        SignalHelper.SetSerialJoin(assignedPanel, (uint)MainPageInfo.SerialJoins.TextBoxInput, someText);
    }

    // Action to perform when the time button is pressed.
    [Join(MainPageInfo.DigitalJoins.WeatherButtonPress)]
    public void OnTimeButtonPress(SigEventArgs args)
    {
        if (SignalHelper.IsRisingEdge(args))
        {
            CrestronConsole.PrintLine("Time Button Pressed");

            if (isTimeDisplayed)
            {
                SignalHelper.SetDigitalJoin(assignedPanel, (uint)MainPageInfo.DigitalJoins.WeatherWidgetVisibility, false);
                isTimeDisplayed = false;
            }
            else
            {
                SignalHelper.SetDigitalJoin(assignedPanel, (uint)MainPageInfo.DigitalJoins.WeatherWidgetVisibility, true);
                isTimeDisplayed = true;
            }

            if (gaugeValue >= 4)
            {
                gaugeValue = 0;
            } 
            else
            {
                gaugeValue++;
            }

            SignalHelper.SetAnalogJoin(assignedPanel, (uint)MainPageInfo.AnalogJoins.GaugeInput, gaugeValue);

            if (someText == "Want A Random Number?")
            {
                someText = random.Next(10000).ToString();
            }
            else
            {
                someText = "Want A Random Number?";
            }

            SignalHelper.SetSerialJoin(assignedPanel, (uint)MainPageInfo.SerialJoins.TextBoxInput, someText);
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
  - `RegisterPanel(panel)` creates a per-panel logic group.
  - `InitializePanel(panel)` runs each page’s `Initialize()` for default UI state.

- **Attribute-driven pages**
  - `[Page(id)]` marks a class as a page; discovered via reflection.
  - Pages are constructed per panel through a `PageFactory`.

- **Automatic join binding**
  - Tag handlers with `[Join(SomeJoinEnum)]` and they’re auto-registered.
  - Join type (Digital/Analog/Serial) is inferred from the **enum name**.

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
  - `SignalHelper.SetDigital/Analog/SerialJoin(...)` for quick panel writes.
  - `SignalHelper.IsRisingEdge(args)` utility for simple button edge detection.
  - Consistent console logging for ops/debug.

- **Clean shutdown**
  - `Cleanup()` clears pools, registries, and dispatchers in order to prevent leaks.

## License

No license. All rights reserved for now.
