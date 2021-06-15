using Godot;

public class KineticOrb : Projectile
{
	private static readonly PackedScene ExplosionScene =
		GD.Load<PackedScene>("res://scenes/game/components/Explosion.tscn");
	
	[Export]
	public uint ExplosiveDamage = 15;
	
	public override void _OnDisappear()
	{
		var explosion = (Explosion) ExplosionScene.Instance();
		explosion.Position = Position;
		explosion.Damage = ExplosiveDamage;
		OwnerWeapon?.OwnerPlayer.ParentWorld.CallDeferred("add_child", explosion);
	}
}
