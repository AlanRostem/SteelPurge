using Godot;
using System;

public class Explosion : Area2D
{
	[Export] public uint Damage = 30;
	[Export] public float KnockBackForce = 100;
	
	private void _OnVulnerableHitBoxHit(object area)
	{
		var hitBox = (VulnerableHitbox)area;
		hitBox.EmitSignal(nameof(VulnerableHitbox.Hit), Damage);
		if (hitBox.GetParent() is Entity entity)
		{
			var angle = Position.AngleToPoint(entity.Position);
			entity.ApplyForce(new Vector2(-Mathf.Cos(angle), -Mathf.Sin(angle)) * KnockBackForce * GetPhysicsProcessDeltaTime());
		}
	}
	
	private void _OnPlayerHit(object body)
	{
		if (body is Player player)
		{
			float damage = 0.5f * Damage;
			player.TakeDamage((uint)damage);
			var angle = Position.AngleToPoint(player.Position);
			var force = new Vector2(-Mathf.Cos(angle) * 0.2f, -Mathf.Sin(angle)) * KnockBackForce *
						GetPhysicsProcessDeltaTime();
			GD.Print(force);
			player.ApplyForce(force);
		}
	}

	private void OnDisappear()
	{
		QueueFree();
	}
}
