using Godot;
using System;

public class KnifeFiringDevice : FiringDevice
{
	private static readonly PackedScene KnifeScene =
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/falcon/BallisticKnife.tscn");
	
	public override void OnFireOutput()
	{
		FireProjectile(KnifeScene);
	}
}
