using Godot;
using System;

public class KineticOrb : Projectile
{
	private static readonly PackedScene ExplosionScene =
		GD.Load<PackedScene>("res://scenes/game/components/Explosion.tscn");
	public override void _OnDisappear()
	{
		var explosion = (Explosion) ExplosionScene.Instance();
		explosion.Position = Position;
		OwnerWeapon.OwnerPlayer.ParentWorld.CallDeferred("add_child",explosion);
	}
}
