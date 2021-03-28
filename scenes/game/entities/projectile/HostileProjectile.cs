using Godot;
using System;

public class HostileProjectile : KinematicBody2D
{
	[Export] public float DirectionAngle = 0;
	[Export] public int DamageDirection = 0;
	[Export] public float MaxVelocity = 250;
	[Export] public uint Damage = 15;
	private bool _hasDisappeared = false;
	
	public Vector2 Velocity;

	public void Init()
	{
		var angle = Mathf.Deg2Rad(DirectionAngle);
		Velocity = new Vector2(
			MaxVelocity * Mathf.Cos(angle),
			MaxVelocity * Mathf.Sin(angle));
	}

	public override void _PhysicsProcess(float delta)
	{
		MoveAndCollide(Velocity * delta);
	}
	
	private void _OnPlayerHit(object body)
	{
		var player = (Player) body;
		player.TakeDamage(Damage, DamageDirection);
		if (!_hasDisappeared)
		{
			_hasDisappeared = true;
			_OnDisappear();
			QueueFree();
		}
	}

	private void _OnHitTileMap(object body)
	{
		if (!_hasDisappeared)
		{
			_hasDisappeared = true;
			_OnDisappear();
			QueueFree();
		}
	}

	public virtual void _OnDisappear()
	{
	}
	
	private void _OnDelete()
	{
		QueueFree();
	}
}
