using Godot;
using System;
using System.Linq;
using Godot.Collections;
using Array = Godot.Collections.Array;

public class WeaponWheel : Control
{
	private static readonly PackedScene WeaponButtonScene =
		GD.Load<PackedScene>("res://scenes/game/ui/inventory/WeaponButton.tscn");

	private int _weaponCount = (int)Inventory.InventoryWeapon.Count;
	private GridContainer _gridContainer;
	private readonly Array<WeaponButton> _buttons = new Array<WeaponButton>();
	
	public override void _Ready()
	{
		// Visible = false;
		_gridContainer = GetNode<GridContainer>("GridContainer");
		_gridContainer.Columns = _weaponCount;
		for (var i = 0; i < _weaponCount; i++)
		{
			AddWeaponButton((Inventory.InventoryWeapon)i);
		}
		
		// TODO: When implementing save files, make sure to change this
		_buttons.First().GrabFocus();
	}

	public override void _Process(float delta)
	{
		
	}

	private void AddWeaponButton(Inventory.InventoryWeapon weapon)
	{
		var button = (WeaponButton) WeaponButtonScene.Instance();
		button.Weapon = weapon;
		_gridContainer.AddChild(button);
		_buttons.Add(button);
	}
}
