using Godot;

/// <summary>
/// Hitbox component with signals tied to projectiles or hit-scan
/// entities that can hurt it.
/// </summary>
public class VulnerableHitbox : Area2D
{
	[Signal]
	public delegate void Hit(uint damage, int knockBackDirection = 0);

	public void TakeHit(uint damage, int knockBackDirection = 0)
	{
		EmitSignal(nameof(Hit), damage, knockBackDirection);
	}
}
