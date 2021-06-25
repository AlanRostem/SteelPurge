using Godot;

public class KineticOrb : Projectile
{
	private static readonly PackedScene ExplosionScene =
		GD.Load<PackedScene>("res://scenes/game/components/Explosion.tscn");
	
	[Export]
	public uint ExplosiveDamage = 15;
	
	public override void _OnDisappear()
	{
		if (OwnerWeapon == null) return;
		var explosion = OwnerWeapon.OwnerPlayer.ParentWorld.Entities.SpawnStaticEntityDeferred<Explosion>(ExplosionScene, Position);
		explosion.Damage = ExplosiveDamage;
	}
}
