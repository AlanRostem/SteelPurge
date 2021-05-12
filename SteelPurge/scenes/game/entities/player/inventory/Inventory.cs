using Godot;
using System;

public class Inventory : Node2D
{

	public enum InventoryWeapon
	{
		Falcon,
		Firewall,
		Joule,
		Count,
	}

	public enum OrdinanceFuelType
	{
		Gasoline,
		EmCell,
		Count
	}

	private static readonly PackedScene[] WeaponScenes =
	{
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/falcon/Falcon.tscn"),
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/firewall/Firewall.tscn"),
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/ke_6_swarm/KE6Swarm.tscn"),
	};
	
	private static readonly uint MaxGuns = 8;

	[Export]
	public PackedScene DefaultGunScene
		= GD.Load<PackedScene>("res://scenes/game/weapon/weapons/ke_6_swarm/KE6Swarm.tscn");
	
	public Weapon EquippedWeapon => _weapon;

	private Weapon _weapon;
	private InventoryWeapon _weaponId = InventoryWeapon.Count;

	private Player _player;
	private Label _scrapLabel;
	private Label _fuelLabel;
	
	public uint ScrapCount = 0;

	public readonly uint[] OrdinanceFuels =
	{
		40,
		40
	};

	private readonly bool[] _weaponContainer = new bool[(int) InventoryWeapon.Count];

	private OrdinanceFuelType _displayedFuel = OrdinanceFuelType.Gasoline;
	
	public override void _Ready()
	{
		_player = GetParent<Player>();
		_scrapLabel = GetNode<Label>("CanvasLayer/ScrapLabel");
		_fuelLabel = GetNode<Label>("CanvasLayer/FuelLabel");
		
		_scrapLabel.Text = "x" + ScrapCount;
		// _fuelLabel.Text = "x" + OrdinanceFuels[(int)_displayedFuel];
		
		for (var i = 0; i < (int) OrdinanceFuelType.Count; i++)
		{
			// Update UI
		}
		
		AddWeapon(InventoryWeapon.Falcon);
		AddWeapon(InventoryWeapon.Firewall);
		AddWeapon(InventoryWeapon.Joule);
		
		SwitchWeapon(InventoryWeapon.Falcon);
	}

	private void KnowEquippedWeaponFuelType()
	{
		_displayedFuel = _weapon.TacticalEnhancement.FuelType;
		_fuelLabel.Text = "x" + OrdinanceFuels[(int)_displayedFuel];
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

	public void SwitchWeapon(InventoryWeapon weapon)
	{
		if (!HasWeapon(weapon) || weapon == _weaponId) return;
		
		_weapon?.OnSwap();
		_weapon?.QueueFree();
		
		var newWeapon = (Weapon)WeaponScenes[(int)weapon].Instance();
		
		_weapon = newWeapon;
		_weapon.OwnerPlayer = _player;
		_weapon.OnEquip();

		
		_weaponId = weapon;

		CallDeferred("add_child", _weapon);
		CallDeferred(nameof(KnowEquippedWeaponFuelType));
		_player.EmitSignal(nameof(Player.WeaponEquipped), newWeapon);
	}

	public void AddWeapon(InventoryWeapon weapon)
	{
		_weaponContainer[(int) weapon] = true;
	}

	public bool HasWeapon(InventoryWeapon weapon)
	{
		return _weaponContainer[(int) weapon];
	}
	
	public void DrainFuel(OrdinanceFuelType fuelType, uint drainPerTick)
	{
		OrdinanceFuels[(int) fuelType] -= drainPerTick;
		if (_displayedFuel == fuelType)
			_fuelLabel.Text = "x" + OrdinanceFuels[(int)_displayedFuel];
	}
}
