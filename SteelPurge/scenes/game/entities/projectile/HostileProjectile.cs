using Godot;
using System;

public class HostileProjectile : KinematicEntity
{
	[Export] public float DirectionAngle = 0;
	[Export] public int DamageDirection = 0;
	[Export] public float MaxVelocity = 250;
	[Export] public uint Damage = 15;
	private bool _hasDisappeared = false;


	public void InitWithAngularVelocity()
	{
		var angle = Mathf.Deg2Rad(DirectionAngle);
		Velocity = new Vector2(
			MaxVelocity * Mathf.Cos(angle),
			MaxVelocity * Mathf.Sin(angle));
	}

	public void InitWithHorizontalVelocity()
	{
		Velocity = new Vector2(DamageDirection * MaxVelocity, 0);
	}


	public override void _PhysicsProcess(float delta)
	{
		MoveAndCollide(Velocity * delta);
	}


	private void _OnBodyHit(object body)
	{
		if (body is Player player)
		{
			player.TakeDamage(Damage, new Vector2(DamageDirection, 0));
		}

		if (!_hasDisappeared)
		{
			_hasDisappeared = true;
			_OnDisappear();
			QueueFree();
		}
	}


	public virtual void _OnDisappear()
	{
		QueueFree();
	}
}
