using Godot;
using System;

public class BuyStationInfoLabel : Label
{
	private void _OnGetWeaponData(uint price, string name)
	{
		Text = "$" + price + "\n" + name;
	}
}
