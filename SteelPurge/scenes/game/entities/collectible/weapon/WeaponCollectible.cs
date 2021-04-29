using Godot;
using System;

public class WeaponCollectible : FallingCollectible
{
	[Export] public PackedScene WeaponScene = GD.Load<PackedScene>("res://scenes/game/weapon/weapons/firewall/Firewall.tscn");
	public override void OnCollected(Player player)
	{
		player.PlayerInventory.SwitchWeapon((Weapon)WeaponScene.Instance());
	}
}
