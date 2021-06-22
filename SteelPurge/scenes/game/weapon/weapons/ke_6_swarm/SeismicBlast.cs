using Godot;
using System;

public class SeismicBlast : Area2D
{
	// TODO: Add sprite that shows the blast
	public static uint BlastDamage = 80;

	private void _OnEnemyEntered(Enemy enemy)
	{
		enemy.TakeDamage(BlastDamage, Vector2.Zero);
		enemy.ApplyStatusEffect(KinematicEntity.StatusEffectType.Stun);
	}
	
	private void _OnExistenceTimerTimeout()
	{
		QueueFree();
	}
}
