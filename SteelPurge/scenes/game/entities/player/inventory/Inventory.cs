using Godot;
using System;

public class Inventory : Node2D
{
	private static PackedScene WeaponCollectibleScene 
		= GD.Load<PackedScene>("res://scenes/game/entities/collectible/weapon/WeaponCollectible.tscn"); 
	
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
	private readonly Weapon[] _guns = new Weapon[MaxGuns];
	private uint _gunCount = 0;
	
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
		AddWeapon(defaultGun);
		_player.EquippedWeapon = defaultGun;
		_player.KnowInventoryScrapCount(ScrapCount);
		for (var i = 0; i < (int) OrdinanceFuelType._Count; i++)
		{
			_player.KnowInventoryOrdinanceFuelCount(OrdinanceFuels[i], (OrdinanceFuelType)i);
		}
	}

	public void AddWeapon(Weapon weapon)
	{
		if (_gunCount >= MaxGuns) return;
		_guns[_gunCount++] = weapon;
		_player.EmitSignal(nameof(Player.WeaponAddedToInventory), weapon);
	}

	public void PickUpScrap(uint count)
	{
		ScrapCount += count;
		_player.KnowInventoryScrapCount(ScrapCount);
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
		_player.KnowInventoryScrapCount(ScrapCount);
	}


	public void AddOrdinanceFuel(uint count, OrdinanceFuelType type)
	{
		OrdinanceFuels[(int)type] += count;
		_player.KnowInventoryOrdinanceFuelCount(OrdinanceFuels[(int)type], type);
	}

	public void SwitchWeapon(Weapon weapon)
	{
		var item = _player.ParentWorld.Entities.SpawnEntityDeferred<WeaponCollectible>(WeaponCollectibleScene, _player.Position);
		item.Weapon = _player.EquippedWeapon;
		_player.RemoveChild(_player.EquippedWeapon);
		_player.EquippedWeapon = weapon;
	}
}
