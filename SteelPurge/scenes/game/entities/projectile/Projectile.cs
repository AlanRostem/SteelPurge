using Godot;
using System;

public class Projectile : KinematicBody2D
{
	[Export] public float DirectionAngle = 0;
	[Export] public float MaxVelocity = 250;
	[Export] public float Gravity = 600;
	[Export] public bool DeleteOnEnemyHit = true;
	[Export] public bool DeleteOnTileMapHit = true;
	[Export] public float CriticalRaySize = 5f;
	[Export] public float VisualAngle = 0f;
	private bool _hasDisappeared = false;
	
	public Weapon OwnerWeapon { get; private set; }
	public float DirectionSign = 1;
	
	public Vector2 Velocity;

	private RayCast2D _criticalHitRayCast;

	public override void _Ready()
	{
		_criticalHitRayCast = GetNode<RayCast2D>("CriticalHitRayCast");
	}

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
		var angle = Velocity.Angle();
		_criticalHitRayCast.CastTo = new Vector2(
			CriticalRaySize * Mathf.Cos(angle),
			CriticalRaySize * Mathf.Sin(angle));
		MoveAndCollide(Velocity * delta);
		if (_criticalHitRayCast.IsColliding())
		{
			var criticalHitBox = _criticalHitRayCast.GetCollider();
			_OnCriticalHitBoxEntered((CriticalHitbox) criticalHitBox);
		}
	}

	private void _OnCriticalHitBoxEntered(CriticalHitbox hitbox)
	{
		hitbox.TakeHit(OwnerWeapon.DamagePerShot);
		OwnerWeapon.EmitSignal(nameof(Weapon.CriticalDamageDealt), OwnerWeapon.DamagePerShot, hitbox);
		_OnHit();
		if (!_hasDisappeared && DeleteOnEnemyHit)
		{
			_hasDisappeared = true;
			_OnDisappear();
			QueueFree();
		}
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
