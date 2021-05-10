using Godot;
using System;

public class WeaponShopItem : ShopItem
{
	public PackedScene WeaponScene;

	public WeaponShopItem(string name, uint price, string iconTexturePath, string weaponScenePath) : base(name,
		price, ItemType.Weapon, iconTexturePath)
	{
		WeaponScene = GD.Load<PackedScene>(weaponScenePath);
	}

	public override void OnPurchase(Fabricator fabricator, World world, Player player)
	{
		var weapon = (Weapon) WeaponScene.Instance();
		player.PlayerInventory.EquippedWeapon.Drop(world, fabricator.Position + new Vector2(0, -24));
		player.PlayerInventory.EquippedWeapon = weapon;
	}

	public override bool Validate(Player player, Fabricator fabricator)
	{
		return player.PlayerInventory.EquippedWeapon.DisplayName != Name && !fabricator.HasWeaponInCart;
	}
}
