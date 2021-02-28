using Godot;
using System;

public class Inventory : Node2D
{
	private static readonly uint MaxGuns = 8;

	[Export]
	public PackedScene DefaultGunScene
		= GD.Load<PackedScene>("res://scenes/game/weapon/weapons/Judger45.tscn");


	private Player _player;
	private readonly Weapon[] _guns = new Weapon[MaxGuns];
	private uint _gunCount = 0;
	
	public uint ScrapCount = 0;
	public uint XeSlugCount = 0;


	public override void _Ready()
	{
		_player = GetParent<Player>();
		
		// TODO: Implement inventory properly
		var defaultGun = (Weapon) DefaultGunScene.Instance();
		AddWeapon(defaultGun);
		_player.EquippedWeapon = defaultGun;
	}

	public void AddWeapon(Weapon weapon)
	{
		if (_gunCount >= MaxGuns) return;
		_guns[_gunCount++] = weapon;
	}

	public void PickUpGun(Weapon weapon)
	{
		// TODO: Implement
	}

	[Signal]
	public delegate void WeaponAdded(Weapon weapon);
}
