using Godot;
using System;

public class PlayerHitBox : Area2D
{
	public Player Player { get; private set; }
	
	[Signal]
	public delegate void Hit(uint damage, float direction);

	public override void _Ready()
	{
		Player = GetParent<Player>();
	}
}
