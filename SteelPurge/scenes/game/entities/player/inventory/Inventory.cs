using Godot;
using System;

public class Inventory : Node2D
{

	public enum InventoryWeapon
	{
		P336,
		Firewall,
		Joule,
		Falcon,
		Count,
	}

	private static readonly PackedScene FloatingNumberScene =
		GD.Load<PackedScene>("res://scenes/game/ui/FloatingTempText.tscn");
	
	private static readonly PackedScene[] WeaponScenes =
	{
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/hamilton_p336/HamitonP336.tscn"),
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/firewall/Firewall.tscn"),
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/ke_6_swarm/KE6Swarm.tscn"),
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/falcon/Falcon.tscn"),
	};
	
	[Export]
	public InventoryWeapon DefaultGun = InventoryWeapon.P336;
	
	public Weapon EquippedWeapon => _weapon;
	public InventoryWeapon EquippedWeaponEnum => _weaponId;

	private Weapon _weapon;
	private InventoryWeapon _weaponId = InventoryWeapon.Count;

	private Player _player;
	private Label _killLabel;
	private CanvasLayer _canvas;
	private FloatingTempText _scrapAddedNumber;
	
	public uint KillCount = 0;

	public uint MaxOrdinanceFuel = 125;
	
	private readonly uint[] _ordinanceFuels =
	{
		125,
		125,
		125,
		125
	};

	private readonly bool[] _weaponContainer = new bool[(int) InventoryWeapon.Count];
	
	public override void _Ready()
	{
		_player = GetParent<Player>();
		_killLabel = GetNode<Label>("CanvasLayer/KillLabel");
		_canvas = GetNode<CanvasLayer>("CanvasLayer");

		// TODO: When implementing save files, make sure to change this
		EquipWeapon(DefaultGun);
	}

	public uint GetOrdinanceFuel(InventoryWeapon weapon)
	{
		return _ordinanceFuels[(int) weapon];
	}
	
	public void EquipWeapon(InventoryWeapon weapon)
	{
		if (HasWeapon(weapon))
		{
			_weapon.RefillAmmo();
			return;
		}
		
		var newWeapon = (Weapon)WeaponScenes[(int)weapon].Instance();
		
		_weapon = newWeapon;
		_weapon.OwnerPlayer = _player;
		_weapon.OnEquip();

		_weaponId = weapon;
		// _weaponLabel.Text = _weapon.DisplayName;

		// _ammoLabel.Text = "x" + _weapon.Ammo;
		
		CallDeferred("add_child", _weapon);
		_player.EmitSignal(nameof(Player.WeaponEquipped), newWeapon);
	}

	public void SwitchWeapon(InventoryWeapon weapon)
	{
		if (HasWeapon(weapon))
		{
			_weapon.RefillAmmo();
			_weapon.TacticalEnhancement?.ReCharge();
			return;
		}
		
		_weapon?.OnSwap();
		_weapon?.QueueFree();
		
		var newWeapon = (Weapon)WeaponScenes[(int)weapon].Instance();
		
		_weapon = newWeapon;
		_weapon.OwnerPlayer = _player;
		_weapon.OnSwitchTo();

		_weaponId = weapon;
		// _weaponLabel.Text = _weapon.DisplayName;

		// _ammoLabel.Text = "x" + _weapon.Ammo;

		CallDeferred("add_child", _weapon);
		_player.EmitSignal(nameof(Player.WeaponEquipped), newWeapon);
	}
	
	public bool HasWeapon(InventoryWeapon weapon)
	{
		return _weaponId == weapon;
	}

	public void IncrementKillCount()
	{
		KillCount++;
		_killLabel.Text = "x" + KillCount;
	}

	private void _ScrapAddedNumberDisappeared()
	{
		_scrapAddedNumber = null;
	}
}
