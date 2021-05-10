using Godot;
using System;

public class Inventory : Node2D
{
	
	public enum OrdinanceFuelType
	{
		Gasoline,
		EmSlug,
		_Count
	}
	
	private static readonly uint MaxGuns = 8;

	[Export]
	public PackedScene DefaultGunScene
		= GD.Load<PackedScene>("res://scenes/game/weapon/weapons/ke_6_swarm/KE6Swarm.tscn");

	private Player _player;
	
	public uint ScrapCount = 0;

	public uint[] OrdinanceFuels =
	{
		40,
		40
	};

	public override void _Ready()
	{
		_player = GetParent<Player>();
		
		// TODO: Implement inventory properly
		var defaultGun = (Weapon) DefaultGunScene.Instance();
		_player.EquippedWeapon = defaultGun;
		// TODO: Update UI for scrap
		for (var i = 0; i < (int) OrdinanceFuelType._Count; i++)
		{
			// Update UI
		}
	}
	

	public void PickUpScrap(uint count)
	{
		ScrapCount += count;
		// TODO: Update UI
	}
	
	public void LoseScrap(uint count)
	{
		if (ScrapCount < count)
		{
			ScrapCount = 0;
		}
		else
		{
			ScrapCount -= count;
		}
		// TODO: Update UI
	}


	public void AddOrdinanceFuel(uint count, OrdinanceFuelType type)
	{
		OrdinanceFuels[(int)type] += count;
		// TODO: Update UI
	}

	public void SwitchWeapon(Weapon weapon)
	{
		_player.EquippedWeapon.Drop(_player.ParentWorld, _player.Position);
		_player.EquippedWeapon = weapon;
	}

	public void DrainFuel(OrdinanceFuelType fuelType, uint drainPerTick)
	{
		// TODO: Update UI
		OrdinanceFuels[(int) fuelType] -= drainPerTick;
	}
}
