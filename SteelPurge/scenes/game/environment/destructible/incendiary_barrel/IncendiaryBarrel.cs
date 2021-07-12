using Godot;
using System;

public class IncendiaryBarrel : DestructibleObstacle
{
	private static readonly PackedScene ExplosionScene =
		GD.Load<PackedScene>("res://scenes/game/components/Explosion.tscn");
	
	protected override void OnTakeDamage(uint damage, VulnerableHitbox.DamageType damageType)
	{
		if (damageType is VulnerableHitbox.DamageType.Heat && !HasDisappeared)
		{
			Disappear();
			var explosion = ParentWorld.CurrentSegment.Entities.SpawnStaticEntityDeferred<Explosion>(ExplosionScene, Position);
			explosion.Damage = Enemy.StandardHealth;
			explosion.Radius = CustomTileMap.Size * 4f;
		}
	}
}
