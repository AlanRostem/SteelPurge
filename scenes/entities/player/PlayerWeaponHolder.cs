using Godot;
using System;

public class PlayerWeaponHolder : Node2D
{
	private static readonly uint MaxGuns = 2;

	private static readonly PackedScene DefaultGunScene
		= GD.Load<PackedScene>("res://scenes/weapon/weapons/judger/Judger.tscn");


	private Player _player;
	private readonly Weapon[] _guns = new Weapon[2];
	private uint _equippedGunIdx = 0;
	private uint _gunCount = 0;
	public Weapon EquippedWeapon => _guns[_equippedGunIdx];


	public override void _Ready()
	{
		_player = (Player) GetParent();
		var defaultGun = (Weapon) DefaultGunScene.Instance();
		AddWeapon(defaultGun);
	}

	public void AddWeapon(Weapon weapon)
	{
		_guns[_gunCount++] = weapon;
		weapon.OwnerPlayer = _player;
		AddChild(weapon);
	}

	public void PickUpGun(Weapon weapon)
	{
		if (_gunCount == 1)
		{
			AddWeapon(weapon);
			SwitchGun();
		}
		else
		{
			EquippedWeapon.OnSwap();
			EquippedWeapon.QueueFree();
			_guns[_equippedGunIdx] = weapon;
			weapon.OwnerPlayer = _player;
		}
	}

	public void SwitchGun()
	{
		if (_gunCount == 1)
			return;
		_guns[_equippedGunIdx].OnSwap();
		_equippedGunIdx = (_equippedGunIdx + 1) % MaxGuns;
		_guns[_equippedGunIdx].OnEquip();
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("switch_gun"))
		{
			SwitchGun();
		}
	}
}



