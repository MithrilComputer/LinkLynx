# LinkLynx
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
- Crestron NuGet Packages (via https://nuget.crestron.com/nuget):
	- Crestron.SimplSharp.SDK.Library.2.21.133 or later
	- Crestron.SimplSharp.SDK.Program.2.21.133 or later
	- Crestron.SimplSharp.SDK.ProgramLibrary.2.21.133 or later
- IDE: Visual Studio 2019 or later
- Target Platform: 4-Series processors (SIMPL# Pro projects)

## Usage Example

```csharp

// This is a single logic page example.

[Page(MainPageInfo.PageID)]
internal class MainPageLogic : PageLogicBase
{
    public MainPageLogic(BasicTriList device) : base(device) { }

    // Initializes the logic for the page.
    public override void Initialize()
    {
        PageHelpers.SetSerialJoin(assignedPanel, (uint)MainPageInfo.SerialJoins.FormattedTextBoxValue, "Hello World!"); // Set the output serial to the input
    }

    // Action to perform when the time button is pressed.
    [Join(MainPageInfo.DigitalJoins.TimeButtonPress)]
    public void OnTimeButtonPress(SigEventArgs args)
    {
        if (GHelpers.IsRisingEdge(args))
        {
            CrestronConsole.PrintLine("Time Button Pressed");

            PageHelpers.SetDigitalJoin(assignedPanel, (uint)MainPageInfo.DigitalJoins.TimeButtonEnable, false);
            PageHelpers.SetDigitalJoin(assignedPanel, (uint)MainPageInfo.DigitalJoins.DateAndTimeWidgetVisibility, true);
        }
    }
}

</pre>

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
  - `GHelpers.SetDigital/Analog/SerialJoin(...)` for quick panel writes.
  - `IsRisingEdge(args)` utility for simple button edge detection.
  - Consistent console logging for ops/debug.

- **Clean shutdown**
  - `Cleanup()` clears pools, registries, and dispatchers in order to prevent leaks.

## License

No license. All rights reserved for now.