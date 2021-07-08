using Godot;
using System;

public class FuelShopItem : ShopItem
{
	public PackedScene CollectibleScene;

	public FuelShopItem()
	{
		
	}

	public FuelShopItem(string name, uint price, string iconTexturePath, string collectibleScenePath) : base(name, price, ItemType.Fuel, iconTexturePath)
	{
		CollectibleScene = GD.Load<PackedScene>(collectibleScenePath);
	}

	public override void OnPurchase(Fabricator fabricator, World world, Player player)
	{
		world.CurrentSegment.Entities.SpawnEntityDeferred<FallingCollectible>(
			CollectibleScene,
			fabricator.Position + new Vector2(0, -24));
	}
}
