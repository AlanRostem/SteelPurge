using Godot;
using System;

public class FallingCollectible : KinematicBody2D
{
	private Vector2 _vel;
	public virtual void OnCollected(Player player)
	{
		
	}

	public override void _PhysicsProcess(float delta)
	{
		_vel.y += Entity.Gravity * delta;
		_vel = MoveAndSlide(_vel, Vector2.Up);
	}

	private void _OnPlayerEnter(object body)
	{
		OnCollected((Player)body);
		QueueFree();
	}
}
