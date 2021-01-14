using Godot;
using System;

public class Enemy : KinematicBody2D
{
	private static PackedScene _moneyScene = GD.Load<PackedScene>("res://Money.tscn");
	private static float WalkSpeed = 30;
	private static float EmergeSpeed = 40;
	private static float ClimbSpeed = 50;
	private float _direction = 1;
	private Vector2 _vel;
	private uint _hp = 100;
	private AnimatedSprite _sprite;
	private uint _damagePerHit = 19;
	private Player _playerRef;
	private Map _mapRef;
	private Timer _attackTimer;
	private Area2D _meleeArea;
	[Export] public float MeleeRange;
	public bool Spawning = false;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
		_mapRef = GetTree().Root.GetNode<Map>("Map");
		_hp = _mapRef.EnemyHp;
		_playerRef = _mapRef.GetNode<Player>("Player");
		_attackTimer = GetNode<Timer>("AttackTimer");
		_meleeArea = GetNode<Area2D>("Area2D");
		_sprite.Play("walk");
	}

	public override void _PhysicsProcess(float delta)
	{
		_direction = Mathf.Sign(_playerRef.GlobalPosition.x - GlobalPosition.x);

		_meleeArea.Position = new Vector2(_direction * MeleeRange, 0);
		if (!Spawning)
		{
			_sprite.FlipH = _direction < 0;
			_vel.x = WalkSpeed * _direction;
			_vel.y += Constants.Gravity;
			if (IsOnWall())
				_vel.y = -ClimbSpeed;
			_vel = MoveAndSlide(_vel, Constants.Up);
		}
		else
		{
			var pos = GlobalPosition;
			pos.y -= EmergeSpeed * delta;
			GlobalPosition = pos;
		}

	}

	private void OnPlayerInMeleeRange(object body)
	{
		if (body is Player)
		{
			_attackTimer.Start();
		}
	}

	private void OnPlayerExitMeleeRange(object body)
	{
		if (body is Player)
		{
			_attackTimer.Stop();
		}
	}

	public void TakeDamage(uint damage)
	{
		if (damage >= _hp)
		{
			_hp = 0;
			QueueFree();
			var money = (Money) _moneyScene.Instance();
			money.Amount = 50;
			money.GlobalPosition = GlobalPosition;
			GetTree().Root.GetNode("Map").AddChild(money);
			_mapRef.EnemiesOnMap--;
			_mapRef.CurrentEnemyCount--;
		}
		else
		{
			_hp -= damage;
		}
	}

	private void OnDamagePlayer()
	{
		_playerRef.TakeDamage(_damagePerHit);
	}
	
	private void OnScreenExit()
	{
		if (_hp == 0)
			return;
		_mapRef.EnemiesOnMap--;
		QueueFree();
	}
}
