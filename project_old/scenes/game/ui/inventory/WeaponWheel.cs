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

		if (Input.IsActionJustPressed("weapon_wheel_left"))
		{
			MoveInWheel(-1);
		}

		if (Input.IsActionJustPressed("weapon_wheel_right"))
		{
			MoveInWheel(1);
		}
	}

	private void MoveInWheel(int direction)
	{
		var i = _selectedWeaponIndex + direction;
		if (i == -1)
			i = _buttons.Count - 1;
		if (i == _buttons.Count)
			i = 0;
		
		while (_buttons[i].Disabled)
		{
			if (i == 0 && direction == -1)
			{
				i = _buttons.Count - 1;
				continue;
			}
			if (i == _buttons.Count - 1 && direction == 1)
			{
				i = 0;
				continue;
			}
			i += direction;
		}

		if (i == _selectedWeaponIndex) return;
		_buttons[_selectedWeaponIndex].Pressed = false;
		_selectedWeaponIndex = i;
		_buttons[_selectedWeaponIndex].Pressed = true;
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
