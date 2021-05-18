using Godot;
using System;

public class WeaponShopItem : ShopItem
{
	public Inventory.InventoryWeapon WeaponType;

	public WeaponShopItem()
	{
		
	}
	
	public WeaponShopItem(string name, uint price, string iconTexturePath, Inventory.InventoryWeapon weaponType) : base(name,
		price, ItemType.Weapon, iconTexturePath)
	{
		WeaponType = weaponType;
	}

	public override void OnPurchase(Fabricator fabricator, World world, Player player)
	{
		player.PlayerInventory.AddWeapon(WeaponType);
		player.PlayerInventory.SwitchWeapon(WeaponType);
	}

	public override bool Validate(Player player, Fabricator fabricator)
	{
		return player.PlayerInventory.EquippedWeapon.WeaponType != WeaponType && !fabricator.HasWeaponInCart(WeaponType) && !player.PlayerInventory.HasWeapon(WeaponType);
	}
}
