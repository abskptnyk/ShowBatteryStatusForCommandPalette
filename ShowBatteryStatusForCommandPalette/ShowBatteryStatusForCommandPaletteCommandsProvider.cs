// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace ShowBatteryStatusForCommandPalette;

public partial class ShowBatteryStatusForCommandPaletteCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;
    private readonly ICommandItem _dockBand;

    public ShowBatteryStatusForCommandPaletteCommandsProvider()
    {
        DisplayName = "Show Battery Status For Command Palette";

        IconInfo DisplayIcon = IconHelpers.FromRelativePaths("./Assets/BatteryIcon-light.png", "./Assets/BatteryIcon-dark.png");

        var mainPage = new ShowBatteryStatusForCommandPalettePage();
        _dockBand = new CommandItem(mainPage) { Title = DisplayName };

        _commands = [
            new CommandItem(new CommandItem(mainPage)) { Title = DisplayName, Icon = DisplayIcon },
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }

    public override ICommandItem[]? GetDockBands()
    {
        return [_dockBand];
    }

}
