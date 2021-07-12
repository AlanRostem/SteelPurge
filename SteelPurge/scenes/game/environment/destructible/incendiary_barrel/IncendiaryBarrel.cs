using Godot;
using System;

public class IncendiaryBarrel : DestructibleObstacle
{
	private static readonly PackedScene ExplosionScene =
		GD.Load<PackedScene>("res://scenes/game/components/Explosion.tscn");
	
	protected override void OnTakeDamage(uint damage, VulnerableHitbox.DamageType damageType)
	{
		if (damageType is VulnerableHitbox.DamageType.Heat)
		{
			EmitSignal(nameof(Destroyed));
			ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
			ParentWorld.CurrentSegment.Entities.SpawnStaticEntityDeferred<Explosion>(ExplosionScene, Position);
		}
	}
}
