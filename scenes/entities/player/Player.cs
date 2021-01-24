using Godot;
using System;

public class Player : Entity
{
	public class StatusInfo
	{
		public uint Round = 1;
		public uint Money = 500;
		public uint Health = 100;
	}

	public static readonly uint HealthRegenCount = 15;
	private static readonly float WalkSpeed = 60;
	private static readonly float JumpSpeed = 220;
	public StatusInfo Stats = new StatusInfo();

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

	[Signal]
	public delegate void TriggerRegenCooldown();
	
	[Signal]
	public delegate void CancelRegen();

	public void TakeDamage(uint damage)
	{
		if (damage >= Stats.Health)
		{
			Stats.Health = 0;
			// TODO: Die
		}
		else
		{
			Stats.Health -= damage;
			EmitSignal(nameof(CancelRegen));
			EmitSignal(nameof(TriggerRegenCooldown));
		}
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

	private void _OnRegen()
	{
		if (HealthRegenCount < Stats.Health)
			Stats.Health += HealthRegenCount;
		else
		{
			Stats.Health = 100;
			EmitSignal(nameof(CancelRegen));	
		}
	}
}
