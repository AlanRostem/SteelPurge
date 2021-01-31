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
	private bool _canTakeDamage = true;
	private bool _isStunned = false;
	public float Direction = 1;
	public bool IsWalking = false;
	public bool IsJumping = false;
	public bool IsHoldingTrigger = false;
	public bool DidReload = false;
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

	[Signal]
	public delegate void TriggerDamageReceptionCooldown();

	public void TakeDamage(uint damage, int knockDir)
	{
		EmitSignal(nameof(CancelRegen));
		EmitSignal(nameof(TriggerRegenCooldown));
		
		if (_canTakeDamage)
		{
			_canTakeDamage = false;
			_isStunned = true;
			Velocity = new Vector2(WalkSpeed * 2 * knockDir, -JumpSpeed / 2);
			EmitSignal(nameof(TriggerDamageReceptionCooldown));
		}
		else
		{
			return;
		}

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
	}

	protected override void _OnMovement(float delta)
	{
		bool isOnFloor = IsOnFloor();
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
			if (isOnFloor)
				Velocity.x = 0;
			IsWalking = false;
		}

		if (WeaponHolder.EquippedWeapon.IsFiring)
		{
			Velocity.x *= WeaponHolder.EquippedWeapon.SlowDownMultiplier;
		}

		IsJumping = !isOnFloor;


		if (isOnFloor)
		{
			if (_jump)
				Velocity.y = -JumpSpeed;
		}
	}

	private bool IsActionPressed(string action)
	{
		return Input.IsActionPressed(action) && !_isStunned;
	}

	private bool IsActionJustPressed(string action)
	{
		return Input.IsActionJustPressed(action) && !_isStunned;
	}

	private void _ProcessInput()
	{
		_left = IsActionPressed("left");
		_right = IsActionPressed("right");
		_jump = IsActionPressed("jump");

		if (IsActionJustPressed("aim"))
		{
			EmitSignal(nameof(TriggerAimSwap));
			_aim = !_aim;
			Direction = -Direction;
		}

		DidReload = IsActionJustPressed("reload");
		IsHoldingTrigger = IsActionPressed("fire");
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
	
	private void _OnCanTakeDamage()
	{
		_isStunned = false;
		_canTakeDamage = true;
	}
}
