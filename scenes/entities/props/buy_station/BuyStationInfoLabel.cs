using Godot;
using System;

public class BuyStationInfoLabel : Label
{
    public override void _Ready()
    {
        var station = GetParent<BuyStation>();
        var weapon = station.WeaponToBuy;
        Text = "$" + station.Price + "\n" + weapon.BuyDisplayName;
    }
}
