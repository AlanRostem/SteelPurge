using Godot;
using System;

public class Inventory : Node2D
{

	public enum InventoryWeapon
	{
		H28,
		Firewall,
		Joule,
		Falcon,
		Count,
	}

	private static readonly PackedScene FloatingNumberScene =
		GD.Load<PackedScene>("res://scenes/game/ui/FloatingTempText.tscn");
	
	private static readonly PackedScene[] WeaponScenes =
	{
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/h28_blaster/H28Blaster.tscn"),
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/firewall/Firewall.tscn"),
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/ke_6_swarm/KE6Swarm.tscn"),
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/falcon/Falcon.tscn"),
	};
	
	[Export]
	public InventoryWeapon DefaultGun = InventoryWeapon.H28;
	
	public Weapon EquippedWeapon => _weapon;
	public InventoryWeapon EquippedWeaponEnum => _weaponId;
	public bool WasWeaponAbilityOnCooldown = false;

	private Weapon _weapon;
	private InventoryWeapon _weaponId = InventoryWeapon.Count;

	private Player _player;
	private Label _scrapLabel;
	private Label _fuelLabel;
	private Label _killLabel;
	private CanvasLayer _canvas;
	private FloatingTempText _scrapAddedNumber;
	
	public uint ScrapCount = 0;
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
	
	private WeaponWheel _weaponWheel;
	
	public override void _Ready()
	{
		_player = GetParent<Player>();
		_scrapLabel = GetNode<Label>("CanvasLayer/ScrapLabel");
		_fuelLabel = GetNode<Label>("CanvasLayer/FuelLabel");
		_killLabel = GetNode<Label>("CanvasLayer/KillLabel");
		_weaponWheel = GetNode<WeaponWheel>("CanvasLayer/WeaponWheel");
		_canvas = GetNode<CanvasLayer>("CanvasLayer");
		
		_scrapLabel.Text = "x" + ScrapCount;
		
		AddWeapon(InventoryWeapon.H28);
		
		// TODO: When implementing save files, make sure to change this
		EquipWeapon(DefaultGun);
		_fuelLabel.Text = "x" + GetOrdinanceFuel(EquippedWeaponEnum);

		_weaponWheel.SelectWeapon(_weaponId);
	}

	public uint GetOrdinanceFuel(InventoryWeapon weapon)
	{
		return _ordinanceFuels[(int) weapon];
	}
	
	public void IncreaseOrdinanceFuel(InventoryWeapon weapon, uint amount)
	{
		_ordinanceFuels[(int) weapon] += amount;
		if (_ordinanceFuels[(int) weapon] > MaxOrdinanceFuel)
			_ordinanceFuels[(int) weapon] = MaxOrdinanceFuel;
		
		if (weapon == EquippedWeaponEnum)
			_fuelLabel.Text = "x" + _ordinanceFuels[(int)weapon];
	}
	
	public void DecreaseOrdinanceFuel(InventoryWeapon weapon, uint amount)
	{
		if (amount < _ordinanceFuels[(int) weapon])
			_ordinanceFuels[(int) weapon] -= amount;
		else
			_ordinanceFuels[(int) weapon] = 0;
		
		if (weapon == EquippedWeaponEnum)
			_fuelLabel.Text = "x" + _ordinanceFuels[(int)weapon];
	}
	
	public void PickUpScrap(uint count)
	{
		ScrapCount += count;
		_scrapLabel.Text = "x" + ScrapCount;
		if (_scrapAddedNumber is null)
		{
			var number = (FloatingTempText) FloatingNumberScene.Instance();
			number.RectSize = Vector2.Zero;
			number.Text = "+" + count;
			_canvas.AddChild(number);
			number.RectPosition = _scrapLabel.RectPosition + new Vector2(0, -number.RectSize.y);
			number.Connect(nameof(FloatingTempText.Disappear), this, nameof(_ScrapAddedNumberDisappeared));
			_scrapAddedNumber = number;
		}
		else
		{
			_scrapAddedNumber.RectSize = Vector2.Zero;
			_scrapAddedNumber.Text = "+" + count;
			_scrapAddedNumber.RectPosition = _scrapLabel.RectPosition + new Vector2(0, -_scrapAddedNumber.RectSize.y);
			_scrapAddedNumber.ExistenceTimer.Start();
		}
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

	public void EquipWeapon(InventoryWeapon weapon)
	{
		if (_weapon != null) 
			throw new Exception("Cannot equip weapon: Player already has one!");
		
		var newWeapon = (Weapon)WeaponScenes[(int)weapon].Instance();
		
		_weapon = newWeapon;
		_weapon.OwnerPlayer = _player;
		_weapon.OnEquip();

		_weaponId = weapon;
		_weaponWheel.SelectWeapon(_weaponId);

		_fuelLabel.Text = "x" + GetOrdinanceFuel(_weaponId);
		
		CallDeferred("add_child", _weapon);
		_player.EmitSignal(nameof(Player.WeaponEquipped), newWeapon);
	}

	public void SwitchWeapon(InventoryWeapon weapon)
	{
		if (!HasWeapon(weapon) || weapon == _weaponId) return;
		
		_weapon?.OnSwap();
		_weapon?.QueueFree();
		
		var newWeapon = (Weapon)WeaponScenes[(int)weapon].Instance();
		
		_weapon = newWeapon;
		_weapon.OwnerPlayer = _player;
		_weapon.OnSwitchTo();

		_weaponId = weapon;
		_weaponWheel.SelectWeapon(_weaponId);

		_fuelLabel.Text = "x" + GetOrdinanceFuel(_weaponId);
		
		CallDeferred("add_child", _weapon);
		_player.EmitSignal(nameof(Player.WeaponEquipped), newWeapon);
	}

	public void AddWeapon(InventoryWeapon weapon)
	{
		_weaponContainer[(int) weapon] = true;
		_weaponWheel.EnableWeaponButton(weapon);
	}

	public bool HasWeapon(InventoryWeapon weapon)
	{
		if (weapon == InventoryWeapon.Count)
			return false;
		return _weaponContainer[(int) weapon];
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
