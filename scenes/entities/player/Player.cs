using Godot;
using System;

public class Player : Entity
{
	private static readonly float WalkSpeed = 60;
	private static readonly float JumpSpeed = 220;

	public float Direction = 1;
	public bool IsWalking = false;
	
	public override void _OnMovement(float delta)
	{
		bool left = Input.IsActionPressed("left");
		bool right = Input.IsActionPressed("right");
		bool jump = Input.IsActionPressed("jump");


		if (left && !right)
		{
			Velocity.x = -WalkSpeed;
			Direction = -1;
			IsWalking = true;
		}
		else if (right && !left)
		{
			Velocity.x = WalkSpeed;
			Direction = 1;
			IsWalking = true;
		}
		else
		{
			Velocity.x = 0;
			IsWalking = false;
		}

		if (jump && IsOnFloor())
		{
			Velocity.y = -JumpSpeed;
		}
	}
}
