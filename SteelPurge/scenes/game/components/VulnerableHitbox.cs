using Godot;

/// <summary>
/// Hitbox component with signals tied to projectiles or hit-scan
/// entities that can hurt it.
/// </summary>
public class VulnerableHitbox : Area2D
{
	public enum DamageType
	{
		Standard,
		Projectile,
		HitScan,
		RamSlide,
		Melee,
		Explosive,
	}
	
	[Signal]
	public delegate void Hit(uint damage, Vector2 knockBackDirection);

	[Export] public bool TakeSlideHits = true;
	[Export] public bool TakeProjectileHits = true;
	
	public void TakeHit(uint damage, DamageType damageType)
	{
		TakeHit(damage, Vector2.Zero, damageType);
	}
	
	public virtual void TakeHit(uint damage, Vector2 knockBackDirection, DamageType damageType)
	{
		EmitSignal(nameof(Hit), damage, knockBackDirection);
	}
}
