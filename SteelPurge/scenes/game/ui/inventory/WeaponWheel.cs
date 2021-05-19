using Godot;
using System;
using System.Linq;
using Godot.Collections;
using Array = Godot.Collections.Array;

public class WeaponWheel : Control
{
	private static readonly PackedScene WeaponButtonScene =
		GD.Load<PackedScene>("res://scenes/game/ui/inventory/WeaponButton.tscn");

	private int _weaponCount = (int) Inventory.InventoryWeapon.Count;
	private int _selectedWeaponIndex = (int) Inventory.InventoryWeapon.Count;
	private GridContainer _gridContainer;
	private readonly Array<WeaponButton> _buttons = new Array<WeaponButton>();
	private Inventory _parent;
	private PauseObject _pauseObject = new PauseObject();

	public override void _Ready()
	{
		Visible = false;
		_parent = GetParent().GetParent<Inventory>();
		_gridContainer = GetNode<GridContainer>("GridContainer");
		_gridContainer.Columns = _weaponCount;
		for (var i = 0; i < _weaponCount; i++)
		{
			AddWeaponButton((Inventory.InventoryWeapon) i);
		}
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("weapon_wheel"))
		{
			_pauseObject.TryToPause(GetTree());
			Visible = _pauseObject.IsPaused;
		}

		if (Input.IsActionJustReleased("weapon_wheel"))
		{
			if (_pauseObject.IsPaused)
			{
				_pauseObject.TryToUnpause(GetTree());
				Visible = false;
				_parent.SwitchWeapon((Inventory.InventoryWeapon) _selectedWeaponIndex);
			}
		}

		if (!Visible) return;

		if (Input.IsActionJustPressed("ui_left"))
		{
			_buttons[_selectedWeaponIndex].Pressed = false;
			if (_selectedWeaponIndex - 1 == -1) _selectedWeaponIndex = _buttons.Count - 1;
			else _selectedWeaponIndex--;
			if (_buttons[_selectedWeaponIndex].Disabled)
				_selectedWeaponIndex--;
			_buttons[_selectedWeaponIndex].Pressed = true;
		}

		if (Input.IsActionJustPressed("ui_right"))
		{
			_buttons[_selectedWeaponIndex].Pressed = false;
			if (_selectedWeaponIndex + 1 == _buttons.Count) _selectedWeaponIndex = 0;
			else _selectedWeaponIndex++;
			if (_buttons[_selectedWeaponIndex].Disabled)
				_selectedWeaponIndex++;
			_buttons[_selectedWeaponIndex].Pressed = true;
			// TODO: Loop to find the next valid button
		}
	}

	public void SelectWeapon(Inventory.InventoryWeapon weapon)
	{
		if (_selectedWeaponIndex >= 0 && _selectedWeaponIndex < _buttons.Count)
			_buttons[_selectedWeaponIndex].Pressed = false;
		_selectedWeaponIndex = (int) weapon;
		_buttons[_selectedWeaponIndex].Pressed = true;
	}

	public void EnableWeaponButton(Inventory.InventoryWeapon weapon)
	{
		var index = (int) weapon;
		var button = _buttons[index];
		button.Weapon = weapon;
		button.Disabled = false;
	}

	private void AddWeaponButton(Inventory.InventoryWeapon weapon)
	{
		var button = (WeaponButton) WeaponButtonScene.Instance();
		button.Disabled = true;
		_gridContainer.AddChild(button);
		_buttons.Add(button);
	}
}
