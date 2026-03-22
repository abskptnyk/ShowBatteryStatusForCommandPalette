// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Power;

namespace ShowBatteryStatusForCommandPalette;

internal sealed partial class ShowBatteryStatusForCommandPalettePage : ListPage
{
    private ListItem? _batteryItem;
    private string? _lastIconPath;

    public ShowBatteryStatusForCommandPalettePage()
    {
        Title = "Show Battery Status For Command Palette";
        Name = "Open";

        PowerManager.BatteryStatusChanged += OnBatteryStatusChanged;
        PowerManager.RemainingChargePercentChanged += OnRemainingChargePercentChanged;
    }

    private IconInfo MakeBatteryIcon(int percent, string status)
    {
        double pct = percent / 100.0;
        double fillHeight = 100 * pct;
        double y = 110 - fillHeight;
        string fill = pct > 0.2 ? "#F5A623" : "#E24B4A";

        if (status == "Charging") fill = "#57D156";
        if (status == "Idle" && percent == 100) fill = "#ffffff";

        string svg = $"""
        <svg width="400" height="400" viewBox="0 0 116 116">
           <rect x="44" y="6" width="28" height="4" stroke-width="3" stroke="black" rx="1"/>
           <rect x="32" y="{y:F1}" width="52" height="{fillHeight:F1}" stroke="none" stroke-width="3" fill="{fill}"/>
           <rect x="32" y="10" width="52" height="100" stroke="black" stroke-width="3" rx="4" fill="none"/>
        </svg>
        """;

        string path = Path.Combine(Path.GetTempPath(), $"cmdpal_battery_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}.svg");
        File.WriteAllText(path, svg);

        if (_lastIconPath != null && File.Exists(_lastIconPath))
        {
            File.Delete(_lastIconPath);
        }

        _lastIconPath = path;
        return new IconInfo(path);
    }

    public override IListItem[] GetItems()
    {
        var batteryPercentage = PowerManager.RemainingChargePercent;
        var batteryStatus = PowerManager.BatteryStatus.ToString();

        _batteryItem = new ListItem(new NoOpCommand())
        {
            Title = $"{batteryPercentage}%",
            Subtitle = batteryStatus,
            Icon = MakeBatteryIcon(batteryPercentage, batteryStatus),
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
            _batteryItem.Icon = MakeBatteryIcon(batteryPercentage, batteryStatus);
        }
    }

    public void Dispose()
    {
        PowerManager.BatteryStatusChanged -= OnBatteryStatusChanged;
        PowerManager.RemainingChargePercentChanged -= OnRemainingChargePercentChanged;
    }
}