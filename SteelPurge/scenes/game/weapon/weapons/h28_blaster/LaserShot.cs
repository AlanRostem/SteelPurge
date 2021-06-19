using Godot;
using System;

public class LaserShot : Area2D
{
	[Export] public uint Damage = 80;
	
	private void _OnVulnerableHitBoxHit(object area)
	{
		var hitBox = (VulnerableHitbox) area;
		hitBox.TakeHit(Damage, Vector2.Zero);
		// TODO: Possible that this call is needed
		// OwnerWeapon?.EmitSignal(nameof(Weapon.DamageDealt), Damage, hitBox);
	}

	public override void _Process(float delta)
	{
		Modulate = new Color(255, 255, 255, Modulate.a - delta);
	}

	private void _OnExistenceTimeout()
	{
		QueueFree();
	}
}
