using Godot;
using System;

public class PlayerCamera : Camera2D
{
	public float MaxMargin = 0.4f;
	public float MinMargin = 0.08f;
	public float Smoothness = 0.03f;
	private Player _player;

	public override void _Ready()
	{
		_player = GetParent<Player>();
	}
}
