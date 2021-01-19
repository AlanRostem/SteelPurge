using Godot;
using System;

public class Entity : KinematicBody2D
{
	private static float Gravity = 600;
	private static Vector2 Up = new Vector2(0, -1);
	public Vector2 Velocity;
	
	public override void _Ready()
	{
		
	}

	public override void _PhysicsProcess(float delta)
	{
		Velocity.y += Gravity * delta;
		Velocity = MoveAndSlide(Velocity, Up, true);
	}
}
