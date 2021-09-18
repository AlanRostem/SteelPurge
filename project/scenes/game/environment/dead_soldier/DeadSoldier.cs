using Godot;
using System;
using Godot.Collections;

public class DeadSoldier : StaticEntity
{
	private static readonly Texture[] _weaponHeldTextures =
	{
		null, // H28
		GD.Load<Texture>("res://assets/texture/dead_soldier_with_firewall.png"), // Firewall
		GD.Load<Texture>("res://assets/texture/dead_soldier_with_joule.png"), // Joule
		null, // Falcon
		GD.Load<Texture>("res://assets/texture/dead_soldier.png"), // Count (Basically none)
	};
	
	[Export] public Inventory.InventoryWeapon Weapon = Inventory.InventoryWeapon.P336;

	private Sprite _sprite;

	public override void _Init()
	{
		_sprite = GetNode<Sprite>("Sprite");
		_sprite.Texture = _weaponHeldTextures[(int) Weapon];
	}

	public override Dictionary<string, object> ExportEntityData()
	{
		var data = new EntityData(base.ExportEntityData());
		data.SetAny(nameof(Weapon), Weapon);
		return data.GetJson();
	}

	public override void FeedEntityData(Dictionary<string, object> data)
	{
		base.FeedEntityData(data);
		var eData = new EntityData(data);
		Weapon = eData.GetAny<Inventory.InventoryWeapon>(nameof(Weapon));
		_sprite.Texture = _weaponHeldTextures[(int) Weapon];
	}

	private void _OnInteract(Player player)
	{
		player.PlayerInventory.SwitchWeapon(Weapon);
		_sprite.Texture = _weaponHeldTextures[(int) Inventory.InventoryWeapon.Count];
	}
}
