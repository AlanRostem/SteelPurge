using Godot;
using System;

public class FuelLabel : Label
{
	private void _OnFuelChanged(uint count, Inventory.OrdinanceFuelType type)
	{
		Text = "x" + count;
	}	
}
