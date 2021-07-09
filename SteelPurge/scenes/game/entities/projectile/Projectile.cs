using System;
using Godot;
using Godot.Collections;

public class Projectile : KinematicEntity
{
	[Export] public float DirectionAngle = 0;
	[Export] public float MaxVelocity = 250;
	[Export] public bool DeleteOnEnemyHit = true;
	[Export] public bool DeleteOnTileMapHit = true;
	[Export] public float VisualAngle = 0f;
	[Export] public uint Damage;
	private bool _hasDisappeared = false;

	public Weapon OwnerWeapon { get; private set; }
	public Vector2 SpritePosition;
	public float DirectionSign = 1;

	public override void _Ready()
	{
		base._Ready();
		CurrentCollisionMode = CollisionMode.Move;
		GetNode<Sprite>("ProjectileSprite").Position = SpritePosition;
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

	private void _OnCriticalHitBoxEntered(CriticalHitbox hitbox)
	{
		var directionalAngle = Mathf.Rad2Deg(Velocity.Angle());
		var targetAngle = Mathf.Rad2Deg(hitbox.CriticalHitDirection.Angle());
		var angleDiff = (directionalAngle - targetAngle + 180 + 360) % 360 - 180;

		// GD.Print("PROJ: " + directionalAngle);
		// GD.Print("CRIT: " + targetAngle);
		// GD.Print("DIFF: " + angleDiff);

		if (angleDiff > hitbox.CriticalHitAngularMargin || angleDiff < -hitbox.CriticalHitAngularMargin) return;

		hitbox.TakeHit(Damage, VulnerableHitbox.DamageType.Projectile);
		OwnerWeapon?.EmitSignal(nameof(Weapon.CriticalDamageDealt), Damage, hitbox);
		if (!_hasDisappeared && DeleteOnEnemyHit)
		{
			_hasDisappeared = true;
			_OnDisappear();
			ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
		}
	}

	private void _OnVulnerableHitBoxEntered(object area)
	{
		if (area is CriticalHitbox criticalHitbox)
		{
			_OnCriticalHitBoxEntered(criticalHitbox);
			return;
		}

		var hitBox = (VulnerableHitbox) area;
		hitBox.TakeHit(Damage, Vector2.Zero, VulnerableHitbox.DamageType.Projectile);
		OwnerWeapon?.EmitSignal(nameof(Weapon.DamageDealt), Damage, hitBox);
		_OnHit(hitBox);
		if (!_hasDisappeared && DeleteOnEnemyHit)
		{
			_hasDisappeared = true;
			_OnDisappear();
			ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
		}
	}

	private void _OnHitTileMap(object body)
	{
		if (!_hasDisappeared && DeleteOnTileMapHit)
		{
			_hasDisappeared = true;
			_OnDisappear();
			ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
		}
	}

	public void Disappear()
	{
		if (_hasDisappeared || !DeleteOnEnemyHit) return;
		_hasDisappeared = true;
		_OnDisappear();
		ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
	}

	public virtual void _OnHit(VulnerableHitbox subject)
	{
	}

	public virtual void _OnDisappear()
	{
	}

	public virtual void _OnLostVisual()
	{
		ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
	}

	public void _OnParentWeaponLost()
	{
		OwnerWeapon = null;
	}

	public override void FeedEntityData(Dictionary<string, object> data)
	{
		base.FeedEntityData(data);
		var eData = new EntityData(data);
		VisualAngle = eData.GetAny<float>(nameof(VisualAngle));
		DirectionSign = eData.GetAny<float>(nameof(DirectionSign));
	}

	public override Dictionary<string, object> ExportEntityData()
	{
		var data =  new EntityData(base.ExportEntityData());
		data.SetAny(nameof(VisualAngle), VisualAngle);
		data.SetAny(nameof(DirectionSign), DirectionSign);
		return data.GetJson();
	}
}
