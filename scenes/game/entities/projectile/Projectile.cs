using Godot;
using System;

public class Projectile : KinematicBody2D
{
	[Export] public float DirectionAngle = 0;
	[Export] public float MaxVelocity = 1;
	[Export] public float Damage = 10;
	
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
		hitBox.EmitSignal(nameof(VulnerableHitbox.Hit), Damage);
		OwnerWeapon.EmitSignal(nameof(Weapon.DamageDealt), Damage, hitBox);
		QueueFree();
	}

	private void _OnHitTileMap(object body)
	{
		QueueFree();
	}
	
	private void _OnDisappear()
	{
	   QueueFree();
	}
}
