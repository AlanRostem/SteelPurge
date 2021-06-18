using Godot;
using System;

public class Scrap : FallingCollectible
{
	[Export] public uint Count = 10;

	private const uint CountLevel1 = 12;
	private const uint CountLevel2 = 25;
	private const uint CountLevel3 = 50;
	private const uint CountLevel4 = 75;

	private static readonly Texture IconLevel0 = GD.Load<Texture>("res://assets/texture/scrap-drop.png");
	private static readonly Texture IconLevel1 = GD.Load<Texture>("res://assets/texture/scrap_drop_bundle_1.png");
	private static readonly Texture IconLevel2 = GD.Load<Texture>("res://assets/texture/scrap_drop_bundle_2.png");
	private static readonly Texture IconLevel3 = GD.Load<Texture>("res://assets/texture/scrap_drop_bundle_3.png");
	private static readonly Texture IconLevel4 = GD.Load<Texture>("res://assets/texture/scrap_drop_bundle_4.png");

	public override void OnCollected(Player player)
	{
		if (player.Health < player.MaxHealth)
		{
			var diff = player.MaxHealth - player.Health;
			var halfCount = Count / 2;
			if (halfCount <= diff)
			{
				player.Health += halfCount;
				player.PlayerInventory.PickUpScrap(halfCount);
				return;
			}

			player.Health = player.MaxHealth;
			player.PlayerInventory.PickUpScrap(Count - diff);
		}
		else
		{
			player.PlayerInventory.PickUpScrap(Count);
		}
	}

	public void SetCount(uint count)
	{
		Count = count;
		CallDeferred(nameof(EvaluateSprite));
	}

	private void EvaluateSprite()
	{
		var halfCount = Count / 2;
		if (halfCount >= CountLevel4)
			IconSprite.SetDeferred("texture", IconLevel4);
		else if (halfCount >= CountLevel3)
			IconSprite.SetDeferred("texture", IconLevel3);
		else if (halfCount >= CountLevel2)
			IconSprite.SetDeferred("texture", IconLevel2);
		else if (halfCount >= CountLevel1)
			IconSprite.SetDeferred("texture", IconLevel1);
		else
			IconSprite.SetDeferred("texture", IconLevel0);
	}

	private void _OnScrapEntered(object body)
	{
		if (body is Scrap scrap && body != this && IsOnFloor())
		{
			Count += scrap.Count;
			scrap.QueueFree();
			EvaluateSprite();
		}
	}
}
