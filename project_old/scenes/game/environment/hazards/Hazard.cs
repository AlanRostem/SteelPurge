using Godot;
using System;

public class Hazard : StaticEntity
{
	[Export] public uint Damage = 10;
	[Export] public bool InstaKillPlayer = false;
	[Export] public bool InstaKillEnemy = true;
	[Export] public bool TargetEnemies = true;

	private Player _player;

	public override void _Process(float delta)
	{
		if (_player != null && !_player.IsInvulnerable)
			_player.TakeDamage(1, new Vector2(Mathf.Sign(_player.VelocityX), 0));
	}

	protected virtual void _OnEntityTouch(LivingEntity entity)
	{
		if (entity is Player player)
		{
			_player = player;
			player.TakeDamage(1, new Vector2(Mathf.Sign(player.VelocityX), 0));
			if (!player.IsRespawning && InstaKillPlayer)
				player.Die();
		}
		
		if (!TargetEnemies && entity is Enemy)
			return;
		
		if (InstaKillEnemy)
		{
			entity.TakeDamage(entity.MaxHealth, Vector2.Zero);
			return;
		}

		entity.TakeDamage(Damage, new Vector2(-Mathf.Sign(entity.VelocityX), 0));
	}


	private void _OnEntityExit(object body)
	{
		if (body is Player)
			_player = null;
	}
}
