using Godot;
using System;

public class Player : Entity
{
	public static readonly uint HealthRegenCount = 15;
	private static readonly float WalkSpeed = 80;
	private static readonly float JumpSpeed = 220;

	private bool _left = false;
	private bool _right = false;
	private bool _jump = false;
	private bool _aim = false;
	public bool CanTakeDamage = true;
	public bool IsAimingUp = false;
	public bool IsAimingDown = false;
	private bool _isStunned = false;
	public float Direction = 1;
	[Export] public float AimAngle = 0;
	public bool IsWalking = false;
	public bool IsJumping = false;
	public bool IsHoldingTrigger = false;
	public bool DidReload = false;

	private Weapon _weapon;
	public Weapon EquippedWeapon
	{
		get => _weapon;
		set
		{
			_weapon?.QueueFree();
			_weapon = value;
			_weapon.OwnerPlayer = this;
			// TODO: Call AddChild normally after tests
			CallDeferred("add_child", _weapon);
			EmitSignal(nameof(WeaponEquipped), value);
		}
	}


	public override void _Ready()
	{
		base._Ready();
		Health = 100;
	}

	[Signal]
	public delegate void WeaponEquipped(Weapon weapon);

	[Signal]
	public delegate void WeaponClipChanged(uint clip);

	[Signal]
	private delegate void TriggerAimSwap();

	[Signal]
	private delegate void TriggerRegenCooldown();

	[Signal]
	private delegate void CancelRegen();

	[Signal]
	private delegate void TriggerDamageReceptionCooldown();

	[Signal]
	private delegate void TriggerInvincibility();

	public void KnowWeaponClipAmmo(uint ammo)
	{
		EmitSignal(nameof(WeaponClipChanged), ammo);
	}
	
	public void TakeDamage(uint damage, int knockDir)
	{
		EmitSignal(nameof(CancelRegen));
		EmitSignal(nameof(TriggerRegenCooldown));

		if (CanTakeDamage)
		{
			CanTakeDamage = false;
			_isStunned = true;
			Velocity = (new Vector2(WalkSpeed * 2 * knockDir, -JumpSpeed / 2));
			EmitSignal(nameof(TriggerDamageReceptionCooldown));
			EmitSignal(nameof(TriggerInvincibility));
		}
		else
		{
			return;
		}

		if (damage >= Health)
		{
			Health = 0;
			// TODO: Die
		}
		else
		{
			Health -= damage;
			//EmitSignal(nameof(CancelRegen));
			//EmitSignal(nameof(TriggerRegenCooldown));
		}
	}

	protected override void _OnMovement(float delta)
	{
		bool isOnFloor = IsOnFloor();
		_ProcessInput();

		var canSwapDirOnMove = !EquippedWeapon.IsFiring && !_aim || IsAimingUp || IsAimingDown;

		if (_left && !_right)
		{
			MoveX(-WalkSpeed);
			if (canSwapDirOnMove)
				Direction = -1;
			IsWalking = true;
		}

		else if (_right && !_left)
		{
			MoveX(WalkSpeed);
			if (canSwapDirOnMove)
				Direction = 1;
			IsWalking = true;
		}
		else
		{
			if (isOnFloor)
				Velocity = new Vector2(0, Velocity.y);
			IsWalking = false;
		}

		if (EquippedWeapon.IsFiring && isOnFloor && !IsAimingUp && !IsAimingDown)
		{
			var velocity = new Vector2(Velocity);
			velocity.x *= EquippedWeapon.SlowDownMultiplier;
			Velocity = velocity;
		}

		IsJumping = !isOnFloor;


		if (!isOnFloor) return;

		if (IsWalking)
			IsAimingDown = false;
		if (_jump)
			MoveY(-JumpSpeed);
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

		if (!EquippedWeapon.IsFiring && IsActionJustPressed("aim_down"))
		{
			//IsAimingUp = IsActionPressed("aim_up");
			IsAimingDown = !IsAimingDown;
		}

		if (IsActionJustPressed("aim"))
		{
			EmitSignal(nameof(TriggerAimSwap));
			_aim = !_aim;
			Direction = -Direction;
			if (!IsJumping && IsAimingDown)
				IsAimingDown = false;
		}

		DidReload = IsActionJustPressed("reload");
		IsHoldingTrigger = IsActionPressed("fire");
	}


	private void _OnRegen()
	{
		if (HealthRegenCount + Health < 100)
		{
			Health+=  HealthRegenCount;
		}
		else
		{
			Health = 100;
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
	}

	private void _OnInvincibilityEnd()
	{
		CanTakeDamage = true;
	}
}
