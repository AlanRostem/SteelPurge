using Godot;
using System;

public class WeaponCollectible : FallingCollectible
{
	[Export] public PackedScene WeaponScene;

	private Weapon _weapon;

	public Weapon Weapon
	{
		get => _weapon;
		set
		{
			_weapon = value;
			
			AddChild(_weapon); // TODO: This might be a source of some further bugs
			
			_weapon.OnSwap();
			var sprite = GetNode<Sprite>("Sprite"); // TODO: Possibly bad
			sprite.Texture = _weapon.CollectibleSprite;
		}
	}
	
	public override void OnCollected(Player player)
	{
		if (_weapon is null)
		{
			if (WeaponScene != null)
				player.PlayerInventory.SwitchWeapon((Weapon) WeaponScene.Instance());
			return;
		}

		RemoveChild(_weapon);
		player.PlayerInventory.SwitchWeapon(_weapon);
		_weapon = null;
	}

	private void _OnTreeExited()
	{
		_weapon?.QueueFree();
	}
}
