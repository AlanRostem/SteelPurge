using Godot;
using System;
using Object = Godot.Object;

public class Player : Entity
{
    public static readonly uint HealthRegenCount = 15;
    private static readonly float MaxWalkSpeed = 80;
    private static readonly float WalkSpeedGround = 330;
    private static readonly float WalkSpeedAir = 40;
    private static readonly float JumpSpeed = 220;

    public float MaxSlideMagnitude = 320;
    public float MaxCrouchSpeed = 20;
    public float CurrentSlideMagnitude = 0;
    public float SlideFriction = 0.1f;

    public float CurrentMaxSpeed = MaxWalkSpeed;

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
    public bool IsSliding = false;

    private Weapon _weapon;
    public Inventory PlayerInventory;

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
        PlayerInventory = GetNode<Inventory>("Inventory");
    }

    [Signal]
    public delegate void WeaponEquipped(Weapon weapon);

    [Signal]
    public delegate void WeaponClipChanged(uint clip);

    [Signal]
    public delegate void ScrapCountChanged(uint count);

    [Signal]
    public delegate void XeSlugCountChanged(uint count);

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

    public void KnowInventoryScrapCount(uint count)
    {
        EmitSignal(nameof(ScrapCountChanged), count);
    }

    public void KnowInventoryXeSlugCount(uint count)
    {
        EmitSignal(nameof(XeSlugCountChanged), count);
    }

    public void TakeDamage(uint damage, int knockDir)
    {
        EmitSignal(nameof(CancelRegen));
        EmitSignal(nameof(TriggerRegenCooldown));

        if (CanTakeDamage)
        {
            CanTakeDamage = false;
            _isStunned = true;
            Velocity = (new Vector2(MaxWalkSpeed * 2 * knockDir, -JumpSpeed / 2));
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

    private void _ProcessInput()
    {
        _left = IsActionPressed("left");
        _right = IsActionPressed("right");
        _jump = IsActionPressed("jump");

        if (IsActionPressed("slide"))
        {
            var velX = Mathf.Abs(Velocity.x);
            if (!IsSliding && IsOnFloor())
            {
                if (velX > 0)
                    CurrentMaxSpeed = MaxSlideMagnitude;
                else
                    CurrentMaxSpeed = MaxCrouchSpeed;
                IsSliding = true;
            }
        }
        else
        {
            IsSliding = false;
        }

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


    protected override void _OnMovement(float delta)
    {
        bool isOnFloor = IsOnFloor();
        _ProcessInput();

        var canSwapDirOnMove = !EquippedWeapon.IsFiring && !_aim || IsAimingUp || IsAimingDown;

        if (IsSliding)
        {
            if (IsOnFloor())
            {
                CurrentMaxSpeed = Mathf.Lerp(CurrentMaxSpeed, MaxCrouchSpeed, SlideFriction);
                Velocity.x = Mathf.Lerp(Velocity.x, Mathf.Sign(Velocity.x) * CurrentMaxSpeed, SlideFriction);
            }
        }
        else
        {
            CurrentMaxSpeed = MaxWalkSpeed;
        }

        if (_left && !_right)
        {
            AccelerateX(-WalkSpeedGround, CurrentMaxSpeed, delta);
            if (canSwapDirOnMove)
                Direction = -1;
            IsWalking = true;
        }

        else if (_right && !_left)
        {
            AccelerateX(WalkSpeedGround, CurrentMaxSpeed, delta);
            if (canSwapDirOnMove)
                Direction = 1;
            IsWalking = true;
        }
        else
        {
            if (isOnFloor)
            {
                if (!IsSliding)
                {
                    if (Mathf.Abs(Velocity.x) <= MaxWalkSpeed + 0.1f)
                        Velocity.x = Mathf.Lerp(Velocity.x, 0, .95f);
                    else
                        Velocity.x = Mathf.Lerp(Velocity.x, 0, .1f);
                }
            }

            IsWalking = false;
        }

        if (EquippedWeapon.IsFiring && isOnFloor && !IsAimingUp && !IsAimingDown)
        {
            Velocity.x *= EquippedWeapon.SlowDownMultiplier;
        }

        IsJumping = !isOnFloor;


        if (!isOnFloor)
        {
            GravityVector = DefaultGravity;
            return;
        }

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


    public override void _OnCollision(KinematicCollision2D collider)
    {
        if (collider.Normal.y != -1 && IsOnFloor() && !IsSliding)
            GravityVector = -collider.Normal * Gravity;
        else
            GravityVector = DefaultGravity;
    }

    private void _OnRegen()
    {
        if (HealthRegenCount + Health < 100)
        {
            Health += HealthRegenCount;
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