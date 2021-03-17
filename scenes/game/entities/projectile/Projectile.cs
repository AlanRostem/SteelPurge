using Godot;
using System;

public class Projectile : KinematicBody2D
{
	[Export] public float DirectionAngle = 0;
	[Export] public float MaxVelocity = 5;
	private bool _hasDisappeared = false;
	
	public Weapon OwnerWeapon { get; private set; }

	public Vector2 Velocity;

	public void Init(Weapon owner)
	{
		var angle = Mathf.Deg2Rad(DirectionAngle);
		Velocity = new Vector2(
			MaxVelocity * Mathf.Cos(angle),
			MaxVelocity * Mathf.Sin(angle));
		OwnerWeapon = owner;
	}

	public override void _PhysicsProcess(float delta)
	{
		MoveAndCollide(Velocity);
	}
	
	private void _OnVulnerableHitBoxEntered(object area)
	{
		var hitBox = (VulnerableHitbox) area;
		hitBox.EmitSignal(nameof(VulnerableHitbox.Hit), OwnerWeapon.DamagePerShot);
		OwnerWeapon.EmitSignal(nameof(Weapon.DamageDealt), OwnerWeapon.DamagePerShot, hitBox);
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
