using Godot;
using System;

public class BuyStation : Area2D
{
	public enum GunId
	{
		MG27,
		Judger
	}

	private static PackedScene[] _gunScenes =
	{
		GD.Load<PackedScene>("res://MG27.tscn"),
		GD.Load<PackedScene>("res://Judger.tscn")
	};
	
	[Export] public uint Cost;
	[Export] public GunId GunToBuy = GunId.MG27;
	
	private bool _canBuy = false;
	private Player _playerRef;
	private Label _label;

	public override void _Ready()
	{
		_label = GetNode<Label>("Label");
	}

	public override void _Process(float delta)
	{
		_label.Text = GunToBuy + "\n" + "$" + Cost;
		if (_canBuy && Input.IsActionJustPressed("buy"))
		{
			if (_playerRef.Score >= Cost)
			{
				_playerRef.Score -= Cost;
				var gun = (Gun) _gunScenes[(int) GunToBuy].Instance();
				if (_playerRef.EquippedGun.Name == gun.Name)
				{
					_playerRef.EquippedGun.AmmoCount = gun.ClipSize;
					_playerRef.EquippedGun.ReserveAmmo = gun.ReserveAmmo;
				}
				else
				{
					_playerRef.PickUpGun(gun);
				}
			}
		}
	}

	private void OnPlayerCanBuy(object body)
	{
		if (body is Player player)
		{
			_canBuy = true;
			_playerRef = player;
		}
	}

	private void OnPlayerCannotBuy(object body)
	{
		if (body is Player)
			_canBuy = false;
	}
}
