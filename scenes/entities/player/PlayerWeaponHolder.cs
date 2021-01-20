using Godot;
using System;

public class PlayerWeaponHolder : Node2D
{
	private Player _player;
	public override void _Ready()
	{
		_player = (Player) GetParent();

	}

	public override void _Process(float delta)
	{
		
	}
}
