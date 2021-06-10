using Godot;
using System;

public class Projectile : KinematicEntity
{
	[Export] public float DirectionAngle = 0;
	[Export] public float MaxVelocity = 250;
	[Export] public bool DeleteOnEnemyHit = true;
	[Export] public bool DeleteOnTileMapHit = true;
	[Export] public float CriticalRaySize = 5f;
	[Export] public float VisualAngle = 0f;
	[Export] public uint Damage;
	private bool _hasDisappeared = false;

	public Weapon OwnerWeapon { get; private set; }
	public float DirectionSign = 1;

	private RayCast2D _criticalHitRayCast;

	public override void _Ready()
	{
		_criticalHitRayCast = GetNode<RayCast2D>("CriticalHitRayCast");
		CurrentCollisionMode = CollisionMode.Move;
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

	protected override void _OnMovement(float delta)
	{
		var angle = Velocity.Angle();
		_criticalHitRayCast.CastTo = new Vector2(
			CriticalRaySize * Mathf.Cos(angle),
			CriticalRaySize * Mathf.Sin(angle));
		if (_criticalHitRayCast.IsColliding())
		{
			var criticalHitBox = _criticalHitRayCast.GetCollider();
			_OnCriticalHitBoxEntered((CriticalHitbox) criticalHitBox);
		}
	}

	private void _OnCriticalHitBoxEntered(CriticalHitbox hitbox)
	{
		hitbox.TakeHit(Damage);
		OwnerWeapon.EmitSignal(nameof(Weapon.CriticalDamageDealt), Damage, hitbox);
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
		hitBox.TakeHit(Damage, Vector2.Zero);
		OwnerWeapon.EmitSignal(nameof(Weapon.DamageDealt), Damage, hitBox);
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
