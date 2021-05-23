using Godot;

public class Explosion : Area2D
{
	[Export] public uint Damage = 30;
	[Export] public float KnockBackForce = 100;
	
	private void _OnVulnerableHitBoxHit(object area)
	{
		var hitBox = (VulnerableHitbox)area;
		hitBox.TakeHit(Damage, Vector2.Zero);
		if (hitBox.GetParent() is Enemy entity)
		{
			var angle = Position.AngleToPoint(entity.Position);
			entity.KnockBack(new Vector2(-Mathf.Cos(angle), -Mathf.Sin(angle)), KnockBackForce * GetPhysicsProcessDeltaTime());
		}
	}
	
	// TODO: Move this code to a KineticOrbExplosion scene
	private void _OnPlayerHit(object body)
	{
		if (body is Player player)
		{
			player.TakeDamage(Damage, Vector2.Zero);
			var angle = Position.AngleToPoint(player.Position);
			var force = new Vector2(-Mathf.Cos(angle) * 0.2f, -Mathf.Sin(angle)) * KnockBackForce *
						GetPhysicsProcessDeltaTime();
			player.ApplyForce(force);
		}
	}

	private void OnDisappear()
	{
		QueueFree();
	}
}
