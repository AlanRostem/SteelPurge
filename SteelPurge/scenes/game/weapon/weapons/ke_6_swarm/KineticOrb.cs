using Godot;

public class KineticOrb : Projectile
{
	private static readonly PackedScene ExplosionScene =
		GD.Load<PackedScene>("res://scenes/game/components/Explosion.tscn");
	
	[Export]
	public uint ExplosiveDamage = 15;
	
	public override void _OnDisappear()
	{
		OwnerWeapon?.OwnerPlayer.ParentWorld.CurrentSegment.Entities.SpawnStaticEntityDeferred<Explosion>(ExplosionScene, Position);
	}
}
