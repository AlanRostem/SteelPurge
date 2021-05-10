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
	
	public Weapon EquippedWeapon
	{
		get => _weapon;
		set
		{
			_weapon = value;
			_weapon.OwnerPlayer = _player;
			_weapon.OnEquip();
			// TODO: Call AddChild normally after tests
			CallDeferred("add_child", _weapon);
			_player.EmitSignal(nameof(Player.WeaponEquipped), value);
		}
	}

	private Weapon _weapon;

	private Player _player;
	private Label _scrapLabel;
	private Label _fuelLabel;
	
	public uint ScrapCount = 0;

	public uint[] OrdinanceFuels =
	{
		40,
		40
	};

	private OrdinanceFuelType _displayedFuel = OrdinanceFuelType.Gasoline;
	
	public override void _Ready()
	{
		_player = GetParent<Player>();
		_scrapLabel = GetNode<Label>("CanvasLayer/ScrapLabel");
		_fuelLabel = GetNode<Label>("CanvasLayer/FuelLabel");
		
		_scrapLabel.Text = "x" + ScrapCount;
		_fuelLabel.Text = "x" + OrdinanceFuels[(int)_displayedFuel];
		
		// TODO: Implement inventory properly
		var defaultGun = (Weapon) DefaultGunScene.Instance();
		EquippedWeapon = defaultGun;
		// TODO: Update UI for scrap
		for (var i = 0; i < (int) OrdinanceFuelType._Count; i++)
		{
			// Update UI
		}
	}
	

	public void PickUpScrap(uint count)
	{
		ScrapCount += count;
		_scrapLabel.Text = "x" + ScrapCount;
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
		_scrapLabel.Text = "x" + ScrapCount;
	}


	public void AddOrdinanceFuel(uint count, OrdinanceFuelType type)
	{
		OrdinanceFuels[(int)type] += count;
		if (_displayedFuel == type)
			_fuelLabel.Text = "x" + OrdinanceFuels[(int)_displayedFuel];
	}

	public void SwitchWeapon(Weapon weapon)
	{
		EquippedWeapon.Drop(_player.ParentWorld, _player.Position);
		EquippedWeapon = weapon;
	}

	public void DrainFuel(OrdinanceFuelType fuelType, uint drainPerTick)
	{
		OrdinanceFuels[(int) fuelType] -= drainPerTick;
		if (_displayedFuel == fuelType)
			_fuelLabel.Text = "x" + OrdinanceFuels[(int)_displayedFuel];
	}
}
