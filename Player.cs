using Godot;
using System;

public class Player : KinematicBody2D
{
	[Export]
	public uint Score = 500;
	
	static float _speed = 60;
	static float _jumpSpeed = 350;

	AnimatedSprite _sprite;
	Vector2 _vel;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
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
