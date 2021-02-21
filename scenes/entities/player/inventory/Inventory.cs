using Godot;
using System;

public class Inventory : Node2D
{
	private static readonly uint MaxGuns = 8;

	[Export]
	public PackedScene DefaultGunScene
		= GD.Load<PackedScene>("res://scenes/weapon/weapons/judger/Judger.tscn");


	private Player _player;
	private readonly Weapon[] _guns = new Weapon[MaxGuns];
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
		if (_gunCount >= MaxGuns) return; // TODO: Add to ammo
		_guns[_gunCount++] = weapon;
		weapon.OwnerPlayer = _player;
		AddChild(weapon);
	}

	public void PickUpGun(Weapon weapon)
	{
		// TODO: Implement
	}
}
