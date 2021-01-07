using Godot;
using System;

public class Player : KinematicBody2D
{
	[Export]
	public uint Score = 500;

	private static float _speed = 60;
	private static float _jumpSpeed = 350;

	private AnimatedSprite _sprite;
	private Vector2 _vel;
	private uint _hp = 100;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}

	public uint GetHP()
	{
		return _hp;
	}
	
	public void TakeDamage(uint damage)
	{
		if (damage <= _hp)
			_hp -= damage;
		else 
			;// TODO: Die
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
}
