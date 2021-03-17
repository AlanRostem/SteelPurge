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
			var angle = Position.AngleTo(entity.Position);
			entity.ApplyForce(new Vector2(-Mathf.Cos(angle), -Mathf.Sin(angle)) * KnockBackForce);
		}
	}
	
	private void _OnPlayerHit(object body)
	{
		if (body is Player player)
		{
			float damage = 0.5f * Damage;
			player.TakeDamage((uint)damage);
			var angle = Position.AngleTo(player.Position);
			player.ApplyForce(new Vector2(-Mathf.Cos(angle), -Mathf.Sin(angle)) * KnockBackForce);
		}
	}

	private void OnDisappear()
	{
		QueueFree();
	}
}
