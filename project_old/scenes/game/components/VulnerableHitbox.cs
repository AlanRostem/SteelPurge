using Godot;
using Godot.Collections;

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
		Heat,
	}
	
	[Signal]
	public delegate void Hit(uint damage, Vector2 knockBackDirection, DamageType type);

	[Export] public Array<DamageType> ImmuneDamageTypes = new Array<DamageType>();
	
	public void TakeHit(uint damage, DamageType damageType)
	{
		TakeHit(damage, Vector2.Zero, damageType);
	}
	
	public virtual void TakeHit(uint damage, Vector2 knockBackDirection, DamageType damageType)
	{
		if (IsImmuneToDamageType(damageType)) return;
		EmitSignal(nameof(Hit), damage, knockBackDirection, damageType);
	}

	public bool IsImmuneToDamageType(DamageType damageType)
	{
		return ImmuneDamageTypes.Contains(damageType);
	}
}
