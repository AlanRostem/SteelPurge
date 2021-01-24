using Godot;
using System;

public class BuyStation : Prop
{
	[Export(PropertyHint.ResourceType)] public PackedScene GunScene;
	[Export] public uint Price = 500;

	public Weapon WeaponToBuy;

	[Signal]
	public delegate void SendWeaponData(uint price, string name);
	
	public override void _Ready()
	{
		base._Ready();
		WeaponToBuy = (Weapon)GunScene.Instance();
		EmitSignal(nameof(SendWeaponData), Price, WeaponToBuy.BuyDisplayName);
	}
}
