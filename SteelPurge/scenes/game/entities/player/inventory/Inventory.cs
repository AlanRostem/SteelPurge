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

	private static readonly PackedScene FloatingNumberScene =
		GD.Load<PackedScene>("res://scenes/game/ui/FloatingTempText.tscn");
	
	private static readonly PackedScene[] WeaponScenes =
	{
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/falcon/Falcon.tscn"),
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/firewall/Firewall.tscn"),
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/ke_6_swarm/KE6Swarm.tscn"),
	};
	
	[Export]
	public InventoryWeapon DefaultGun = InventoryWeapon.Falcon;
	
	public Weapon EquippedWeapon => _weapon;
	public InventoryWeapon EquippedWeaponEnum => _weaponId;

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

	public readonly uint[] OrdinanceFuels =
	{
		4000,
		4000
	};

	private readonly bool[] _weaponContainer = new bool[(int) InventoryWeapon.Count];

	private OrdinanceFuelType _displayedFuel = OrdinanceFuelType.Gasoline;

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
		// _fuelLabel.Text = "x" + OrdinanceFuels[(int)_displayedFuel];
		
		for (var i = 0; i < (int) OrdinanceFuelType.Count; i++)
		{
			// Update UI
		}
		
		AddWeapon(InventoryWeapon.Falcon);
		AddWeapon(InventoryWeapon.Firewall);
		AddWeapon(InventoryWeapon.Joule);
		
		// TODO: When implementing save files, make sure to change this
		SwitchWeapon(DefaultGun);
		_weaponWheel.SelectWeapon(_weaponId);
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
		if (_scrapAddedNumber is null)
		{
			var number = (FloatingTempText) FloatingNumberScene.Instance();
			number.RectSize = Vector2.Zero;
			number.Text = "+" + count;
			_canvas.AddChild(number);
			number.RectPosition = _scrapLabel.RectPosition + new Vector2(_scrapLabel.RectSize.x - number.RectSize.x, -_scrapLabel.RectSize.y);
			number.Connect(nameof(FloatingTempText.Disappear), this, nameof(_ScrapAddedNumberDisappeared));
			_scrapAddedNumber = number;
		}
		else
		{
			_scrapAddedNumber.RectSize = Vector2.Zero;
			_scrapAddedNumber.Text = "+" + count;
			_scrapAddedNumber.RectPosition = _scrapLabel.RectPosition + new Vector2(_scrapLabel.RectSize.x - _scrapAddedNumber.RectSize.x, -_scrapLabel.RectSize.y);
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
		_weaponWheel.SelectWeapon(_weaponId);

		CallDeferred("add_child", _weapon);
		CallDeferred(nameof(KnowEquippedWeaponFuelType));
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
	
	public void DrainFuel(OrdinanceFuelType fuelType, uint drainPerTick)
	{
		OrdinanceFuels[(int) fuelType] -= drainPerTick;
		if (_displayedFuel == fuelType)
			_fuelLabel.Text = "x" + OrdinanceFuels[(int)_displayedFuel];
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
