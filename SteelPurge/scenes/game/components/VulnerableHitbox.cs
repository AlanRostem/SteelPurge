using Godot;

/// <summary>
/// Hitbox component with signals tied to projectiles or hit-scan
/// entities that can hurt it.
/// </summary>
public class VulnerableHitbox : Area2D
{
	[Signal]
	public delegate void Hit(uint damage, Vector2 knockBackDirection);

	public virtual void TakeHit(uint damage, Vector2 knockBackDirection)
	{
		EmitSignal(nameof(Hit), damage, knockBackDirection);
	}
}
