using Godot;

/// <summary>
/// Hitbox component with signals tied to projectiles or hit-scan
/// entities that can hurt it.
/// </summary>
public class VulnerableHitbox : Area2D
{

	private static readonly PackedScene DamageNumberScene =
		GD.Load<PackedScene>("res://scenes/game/components/DamageNumber.tscn");

	private static readonly Vector2 StartOffset = new Vector2(-16, -32);
	
	private DamageNumber _damageNumber;
	
	[Signal]
	public delegate void Hit(uint damage, Vector2 knockBackDirection);

	public virtual void TakeHit(uint damage, Vector2 knockBackDirection)
	{
		EmitSignal(nameof(Hit), damage, knockBackDirection);
		if (GetParent() is KinematicEntity entity) // TODO: Consider further cases
		{
			if (_damageNumber is null)
			{
				var number = (DamageNumber) DamageNumberScene.Instance();
				number.Damage = damage;
				number.RectPosition = entity.Position + StartOffset;
				entity.ParentWorld.AddChild(number);
				_damageNumber = number;
				number.Connect(nameof(DamageNumber.Disappear), this, nameof(_OnDamageNumberDisappear));
			}
			else
			{
				_damageNumber.RectPosition = entity.Position + StartOffset;
				_damageNumber.Damage += damage;
			}
		}
	}

	private void _OnDamageNumberDisappear()
	{
		_damageNumber = null;
	}
}
