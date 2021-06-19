using Godot;
using System;

public class Inventory : Node2D
{

	public enum InventoryWeapon
	{
		H28,
		Firewall,
		Joule,
		Count,
	}

	private static readonly PackedScene FloatingNumberScene =
		GD.Load<PackedScene>("res://scenes/game/ui/FloatingTempText.tscn");
	
	private static readonly PackedScene[] WeaponScenes =
	{
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/h28_blaster/H28Blaster.tscn"),
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/firewall/Firewall.tscn"),
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/ke_6_swarm/KE6Swarm.tscn"),
	};
	
	[Export]
	public InventoryWeapon DefaultGun = InventoryWeapon.H28;
	
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

	public uint MaxOrdinanceFuel = 125;
	public uint OrdinanceFuel = 125;

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
		_fuelLabel.Text = "x" + OrdinanceFuel;
		
		AddWeapon(InventoryWeapon.H28);
		AddWeapon(InventoryWeapon.Firewall);
		AddWeapon(InventoryWeapon.Joule);
		
		// TODO: When implementing save files, make sure to change this
		SwitchWeapon(DefaultGun);
		_weaponWheel.SelectWeapon(_weaponId);
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


	public void AddOrdinanceFuel(uint count)
	{
		OrdinanceFuel += count;
		_fuelLabel.Text = "x" + OrdinanceFuel;
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
	
	public void DrainFuel(uint drainPerTick)
	{
		OrdinanceFuel -= drainPerTick;
		_fuelLabel.Text = "x" + OrdinanceFuel;
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
