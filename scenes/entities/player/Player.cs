using Godot;
using System;

public class Player : Entity
{
	public class StatusInfo
	{
		public uint Money = 500;
		public uint Health = 100;
	}

	public static readonly uint HealthRegenCount = 15;
	private static readonly float WalkSpeed = 60;
	private static readonly float JumpSpeed = 220;
	public StatusInfo Stats = new StatusInfo();

	private bool _left = false;
	private bool _right = false;
	private bool _jump = false;
	private bool _aim = false;
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
	public delegate void TriggerAimSwap();

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
			//EmitSignal(nameof(CancelRegen));
			//EmitSignal(nameof(TriggerRegenCooldown));
		}

		EmitSignal(nameof(CancelRegen));
		EmitSignal(nameof(TriggerRegenCooldown));
	}

	protected override void _OnMovement(float delta)
	{
		_ProcessInput();

		if (_left && !_right)
		{
			Velocity.x = -WalkSpeed;
			if (!WeaponHolder.EquippedWeapon.IsFiring && !_aim)
				Direction = -1;
			IsWalking = true;
		}

		else if (_right && !_left)
		{
			Velocity.x = WalkSpeed;
			if (!WeaponHolder.EquippedWeapon.IsFiring && !_aim)
				Direction = 1;
			IsWalking = true;
		}

		else
		{
			Velocity.x = 0;
			IsWalking = false;
		}

		if (WeaponHolder.EquippedWeapon.IsFiring)
		{
			Velocity.x *= WeaponHolder.EquippedWeapon.SlowDownMultiplier;
		}

		bool isOnFloor = IsOnFloor();
		IsJumping = !isOnFloor;


		if (isOnFloor)
		{
			if (_jump)
				Velocity.y = -JumpSpeed;
		}
	}

	private void _ProcessInput()
	{
		if (Input.IsActionJustPressed("left"))
		{
			if (!_left)
				_left = true;
		}
		else if (Input.IsActionJustReleased("left"))
		{
			if (_left)
				_left = false;
		}

		if (Input.IsActionJustPressed("right"))
		{
			if (!_right)
				_right = true;
		}
		else if (Input.IsActionJustReleased("right"))
		{
			if (_right)
				_right = false;
		}

		if (Input.IsActionJustPressed("jump"))
		{
			if (!_jump)
				_jump = true;
		}
		else if (Input.IsActionJustReleased("jump"))
		{
			if (_jump)
				_jump = false;
		}

		if (Input.IsActionJustPressed("aim"))
		{
			EmitSignal(nameof(TriggerAimSwap));
			_aim = !_aim;
			Direction = -Direction;
		}
	}

	private void _OnRegen()
	{
		if (HealthRegenCount + Stats.Health < 100)
		{
			Stats.Health += HealthRegenCount;
		}
		else
		{
			Stats.Health = 100;
			EmitSignal(nameof(CancelRegen));
		}
	}

	private void _OnSwapTimeOver()
	{
		_aim = false;
	}
}
