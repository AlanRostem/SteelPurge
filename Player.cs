using Godot;
using System;

public class Player : KinematicBody2D
{
	[Export] public uint Score = 500;

	private static float _speed = 60;
	private static float _jumpSpeed = 350;

	private AnimatedSprite _sprite;
	private Vector2 _vel;
	private uint _hp = 100;

	private Timer _regenTickTimer;
	private Timer _regenStartDelayTimer;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
		_regenTickTimer = GetNode<Timer>("RegenTickTimer");
		_regenStartDelayTimer = GetNode<Timer>("RegenStartDelayTimer");
	}

	public uint GetHP()
	{
		return _hp;
	}

	public void TakeDamage(uint damage)
	{
		if (damage < _hp)
		{
			_hp -= damage;
			_regenTickTimer.Stop();
			_regenStartDelayTimer.Stop();
			_regenStartDelayTimer.Start();
		}
		else
			; // TODO: Die
	}

	public override void _PhysicsProcess(float delta)
	{
		_vel.y += Constants.Gravity;

		if (Input.IsActionPressed("left"))
		{
			_sprite.Play("run");
			_sprite.FlipH = true;
			_vel.x = -_speed;
		}
		else if (Input.IsActionPressed("right"))
		{
			_sprite.Play("run");
			_sprite.FlipH = false;
			_vel.x = _speed;
		}
		else
		{
			_sprite.Play("idle");
			_vel.x = 0;
		}

		if (Input.IsActionPressed("jump") && IsOnFloor())
		{
			_vel.y = -_jumpSpeed;
			_sprite.Play("jump");
		}

		_vel = MoveAndSlide(_vel, Constants.Up, true);
	}

	private void OnCanRegen()
	{
		_regenTickTimer.Start();
	}

	private void OnHeal()
	{
		const uint amount = 12;
		_hp += amount;
		if (_hp > 100)
		{
			_regenTickTimer.Stop();
			_hp = 100;
		}
	}
}
