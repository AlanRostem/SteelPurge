using Godot;
using System;

public class Money : KinematicBody2D
{
	private Vector2 _vel;
	
	[Export]
	public uint Amount;
	public override void _Ready()
	{
		
	}

	public override void _PhysicsProcess(float delta)
	{
		_vel.y += Constants.Gravity;
		_vel = MoveAndSlide(_vel, Constants.Up);
	}
	private void OnCollect(object body)
	{
		if (body is Player player)
		{
			player.Score += Amount;
			QueueFree();
		}
	}
}



