using Godot;
using System;

public class BuyStation : Prop
{
	[Export(PropertyHint.ResourceType)] public PackedScene GunScene;
	[Export] public uint Price = 500;

	public Weapon WeaponToBuy;

	private bool _canBuy = false;
	private Player _player = null;

	[Signal]
	public delegate void SendWeaponData(uint price, string name);
	
	public override void _Ready()
	{
		base._Ready();
		WeaponToBuy = (Weapon)GunScene.Instance();
		EmitSignal(nameof(SendWeaponData), Price, WeaponToBuy.BuyDisplayName);
	}

	public override void _Process(float delta)
	{
		if (_canBuy)
		{
			if (Input.IsActionJustPressed("buy"))
			{
				// TODO: Buy
			}
		}
	}

	private void _OnPlayerEnter(object body)
	{
		_canBuy = true;
		if (_player != null)
			_player = (Player) body;
	}
	
	private void _OnPlayerLeave(object body)
	{
		_canBuy = false;
	}
}
