using Godot;
using System;

public class WeaponCollectible : FallingCollectible
{
	[Export] public PackedScene WeaponScene =
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/firewall/Firewall.tscn");

	private Weapon _weapon;

	public Weapon Weapon
	{
		get => _weapon;
		set
		{
			_weapon = value;
			_weapon.OnSwap();
			var sprite = GetNode<Sprite>("Sprite"); // TODO: Possibly bad
			sprite.Texture = _weapon.CollectibleSprite;
		}
	}

	public override void OnCollected(Player player)
	{
		player.PlayerInventory.SwitchWeapon((Weapon) WeaponScene.Instance());
		_weapon = null;
	}

	private void _OnTreeExited()
	{
		_weapon?.QueueFree();
	}
}
