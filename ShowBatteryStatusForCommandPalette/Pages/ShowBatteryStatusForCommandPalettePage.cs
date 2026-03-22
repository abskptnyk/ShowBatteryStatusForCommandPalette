// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Windows.System.Power;

namespace ShowBatteryStatusForCommandPalette;

internal sealed partial class ShowBatteryStatusForCommandPalettePage : ListPage
{
    private ListItem? _batteryItem;

    public ShowBatteryStatusForCommandPalettePage()
    {
        Title = "Show Battery Status For Command Palette";
        Name = "Open";

        PowerManager.BatteryStatusChanged += OnBatteryStatusChanged;
        PowerManager.RemainingChargePercentChanged += OnRemainingChargePercentChanged;
    }

    public override IListItem[] GetItems()
    {
        var batteryPercentage = PowerManager.RemainingChargePercent;
        var batteryStatus = PowerManager.BatteryStatus.ToString();
        IconInfo DisplayIcon = IconHelpers.FromRelativePaths("./Assets/BatteryIcon-light.png", "./Assets/BatteryIcon-dark.png");


        _batteryItem = new ListItem(new NoOpCommand())
        {
            Title = $"{batteryPercentage}%",
            Subtitle = batteryStatus,
            Icon = DisplayIcon,
        };

        return [_batteryItem];
    }

    private void OnBatteryStatusChanged(object? sender, object e)
    {
        UpdateBatteryDisplay();
    }

    private void OnRemainingChargePercentChanged(object? sender, object e)
    {
        UpdateBatteryDisplay();
    }

    private void UpdateBatteryDisplay()
    {
        var batteryPercentage = PowerManager.RemainingChargePercent;
        var batteryStatus = PowerManager.BatteryStatus.ToString();

        if (_batteryItem != null)
        {
            _batteryItem.Title = $"{batteryPercentage}%";
            _batteryItem.Subtitle = batteryStatus;
        }
    }

    public void Dispose()
    {
        PowerManager.BatteryStatusChanged -= OnBatteryStatusChanged;
        PowerManager.RemainingChargePercentChanged -= OnRemainingChargePercentChanged;
    }
}
