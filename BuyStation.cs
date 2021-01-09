using Godot;
using System;

public class BuyStation : Area2D
{
	public enum GunId
	{
		MG27
	}

	private static PackedScene[] _gunScenes =
	{
		GD.Load<PackedScene>("res://MG27.tscn")
	};
	
	[Export] public uint Cost;
	[Export] public GunId GunToBuy = GunId.MG27;
	
	private bool _canBuy = false;
	private Player _playerRef;

	public override void _Ready()
	{
	}

	public override void _Process(float delta)
	{
		if (_canBuy && Input.IsActionJustPressed("buy"))
		{
			if (_playerRef.Score >= Cost)
			{
				_playerRef.Score -= Cost;
				_playerRef.PickUpGun((Gun)_gunScenes[(int)GunToBuy].Instance());
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
