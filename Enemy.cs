using Godot;
using System;

public class Enemy : KinematicBody2D
{
	private static float WalkSpeed = 40;
	private float _direction = 1;
	private Vector2 _vel;
	private uint HP = 100;
	private AnimatedSprite _sprite;
	private uint _damagePerHit = 34;
	private Player _playerRef;
	private Timer _attackTimer;
	
	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
		_attackTimer = GetNode<Timer>("AttackTimer");
		_sprite.Play("walk");
	}

	public override void _PhysicsProcess(float delta)
	{
		if (IsOnWall())
			_direction *= -1;
		_sprite.FlipH = _direction < 0;
		_vel.x = WalkSpeed * _direction;
		_vel.y += Constants.Gravity;
		_vel = MoveAndSlide(_vel, Constants.Up);
	}
	
	private void OnPlayerInMeleeRange(object body)
	{
		if (body is Player player)
		{
			_playerRef = player;
			_attackTimer.Start();
		}
	}
	
	private void OnPlayerExitMeleeRange(object body)
	{
		if (body is Player player)
		{
			_attackTimer.Stop();
		}
	}
	
	private void OnDamagePlayer()
	{
		_playerRef.TakeDamage(_damagePerHit);
	}
}
