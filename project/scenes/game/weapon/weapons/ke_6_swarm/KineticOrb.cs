using Godot;

public class KineticOrb : Projectile
{
	private static readonly PackedScene ExplosionScene =
		GD.Load<PackedScene>("res://scenes/game/components/Explosion.tscn");

	public override void _OnDisappear()
	{
		OwnerWeapon?.OwnerPlayer.ParentWorld.CurrentSegment.Entities.SpawnStaticEntityDeferred<Explosion>(ExplosionScene, Position);
	}
}
