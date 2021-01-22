using Godot;
using System;

public class Player : Entity
{
	private static readonly float WalkSpeed = 60;
	private static readonly float JumpSpeed = 220;

	public float Direction = 1;
	public bool IsWalking = false;
	public bool IsJumping = false;
	private PlayerWeaponHolder _holder;

	public PlayerWeaponHolder WeaponHolder => _holder;

	public override void _Ready()
	{
		base._Ready();
		ParentMap.PlayerRef = this;
		_holder = GetNode<PlayerWeaponHolder>("PlayerWeaponHolder");
	}

	protected override void _OnMovement(float delta)
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

		bool isOnFloor = IsOnFloor();
		IsJumping = !isOnFloor;


		if (isOnFloor)
		{
			if (jump)
				Velocity.y = -JumpSpeed;
		}
	}
}
