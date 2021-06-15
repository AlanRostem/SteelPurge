using Godot;
using System;

public class Scrap : FallingCollectible
{
	[Export] public uint Count = 10;
	public override void OnCollected(Player player)
	{
		if (player.Health < player.MaxHealth)
		{
			var heal = Count / 2;
			if (heal == 0)
				heal = 1;
			player.Health += heal;
			if (player.Health != player.MaxHealth) return;
			var diff = player.MaxHealth - player.Health;
			player.PlayerInventory.PickUpScrap(diff);
		}
		else
		{
			player.PlayerInventory.PickUpScrap(Count);
		}
	}
}
