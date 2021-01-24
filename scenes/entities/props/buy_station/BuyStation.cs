using Godot;
using System;

public class BuyStation : Prop
{
	[Export(PropertyHint.ResourceType)] public PackedScene GunScene;
	[Export] public uint Price = 500;

	public Weapon WeaponToBuy;
	
	public override void _Ready()
	{
		base._Ready();
		WeaponToBuy = (Weapon)GunScene.Instance();
	}
}
