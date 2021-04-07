using Godot;
using System;

public class Projectile : KinematicBody2D
{
	[Export] public float DirectionAngle = 0;
	[Export] public float MaxVelocity = 250;
	[Export] public float Gravity = 600;
	[Export] public bool DeleteOnEnemyHit = true;
	[Export] public bool DeleteOnTileMapHit = true;
	private bool _hasDisappeared = false;
	
	public Weapon OwnerWeapon { get; private set; }

	public Vector2 Velocity;

	public void InitWithAngularVelocity(Weapon owner)
	{
		var angle = Mathf.Deg2Rad(DirectionAngle);
		Velocity = new Vector2(
			MaxVelocity * Mathf.Cos(angle),
			MaxVelocity * Mathf.Sin(angle));
		OwnerWeapon = owner;
	}
	
	public void InitWithHorizontalVelocity(Weapon owner)
	{
		Velocity = new Vector2(OwnerWeapon.OwnerPlayer.HorizontalLookingDirection * MaxVelocity, 0);
		OwnerWeapon = owner;
	}

	public override void _PhysicsProcess(float delta)
	{
		Velocity.y += Gravity * delta;
		MoveAndCollide(Velocity * delta);
	}
	
	private void _OnVulnerableHitBoxEntered(object area)
	{
		var hitBox = (VulnerableHitbox) area;
		hitBox.TakeHit(OwnerWeapon.DamagePerShot);
		OwnerWeapon.EmitSignal(nameof(Weapon.DamageDealt), OwnerWeapon.DamagePerShot, hitBox);
		_OnHit();
		if (!_hasDisappeared && DeleteOnEnemyHit)
		{
			_hasDisappeared = true;
			_OnDisappear();
			QueueFree();
		}
	}

	private void _OnHitTileMap(object body)
	{
		if (!_hasDisappeared && DeleteOnTileMapHit)
		{
			_hasDisappeared = true;
			_OnDisappear();
			QueueFree();
		}
	}

	public virtual void _OnHit()
	{
		
	}
	
	public virtual void _OnDisappear()
	{
	}
	
	public virtual void _OnLostVisual()
	{
		QueueFree();
	}
}
