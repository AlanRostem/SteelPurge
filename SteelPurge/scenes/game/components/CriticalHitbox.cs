using Godot;

public class CriticalHitbox : Area2D
{
	[Export] public float CriticalHitMultiplier = 1.5f;

	[Signal]
	public delegate void Hit(uint damage, int knockBackDirection = 0);

	public void TakeHit(uint damage, int knockBackDirection = 0)
	{
		EmitSignal(nameof(Hit), (uint)(damage * CriticalHitMultiplier), knockBackDirection);
	}
}
