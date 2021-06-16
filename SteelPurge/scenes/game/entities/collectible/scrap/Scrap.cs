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
			if (player.Health != player.MaxHealth)
			{
				player.PlayerInventory.PickUpScrap(heal);
				return;
			}
			var diff = player.MaxHealth - player.Health;
			player.PlayerInventory.PickUpScrap(diff);
		}
		else
		{
			player.PlayerInventory.PickUpScrap(Count);
		}
	}
	
	private void _OnScrapEntered(object body)
	{
		if (body is Scrap scrap && body != this && IsOnFloor())
		{
			GD.Print("Scoop!");
			Count += scrap.Count;
			scrap.QueueFree();
		}
	}
}
