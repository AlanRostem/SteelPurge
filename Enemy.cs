using Godot;
using System;

public class Enemy : KinematicBody2D
{
	private static float WalkSpeed = 40;
	private float _direction = 1;
	private Vector2 _vel;
	private uint HP = 100;
	private AnimatedSprite _sprite;
	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
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
}
