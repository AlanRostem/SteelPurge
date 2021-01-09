using System;
using Godot;

public class Player : KinematicBody2D
{
	private static readonly PackedScene _defaultGunScene = GD.Load<PackedScene>("res://MG27.tscn");
	[Export] public uint Score = 500;

	private static float _speed = 60;
	private static float _jumpSpeed = 350;

	private AnimatedSprite _sprite;
	private Vector2 _vel;
	private uint _hp = 100;

	private Timer _regenTickTimer;
	private Timer _regenStartDelayTimer;

	private Gun _gun0;
	private Gun _gun1;
	public Gun EquippedGun;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
		_regenTickTimer = GetNode<Timer>("RegenTickTimer");
		_regenStartDelayTimer = GetNode<Timer>("RegenStartDelayTimer");

		_gun0 = EquippedGun = (Gun) _defaultGunScene.Instance();
		AddChild(_gun0);
	}

	public void SwitchGun()
	{
        if (EquippedGun == _gun0)
		{
			if (_gun1 != null)
			{
				EquippedGun.IsEquipped = false;
				EquippedGun = _gun1;
				EquippedGun.IsEquipped = true;
			}
		}
		else if (EquippedGun == _gun1)
		{
			EquippedGun.IsEquipped = false;
			EquippedGun = _gun0;
			EquippedGun.IsEquipped = true;
		}
	}

	public void PickUpGun(Gun gun)
    {
        EquippedGun.IsEquipped = false;
        if (_gun1 == null)
        {
            _gun1 = gun;
			EquippedGun = _gun1;
            EquippedGun.IsEquipped = true;
            AddChild(_gun1);
			return;
        }
        
		if (EquippedGun == _gun0)
		{
			_gun0.QueueFree();
			_gun0 = gun;
		}
		else if (EquippedGun == _gun1)
		{
			_gun1.QueueFree();
			_gun1 = gun;
		}

		EquippedGun = gun;
        EquippedGun.IsEquipped = true;
    }
	
	public uint GetHP()
	{
		return _hp;
	}

	public void TakeDamage(uint damage)
	{
		if (damage < _hp)
		{
			_hp -= damage;
			_regenTickTimer.Stop();
			_regenStartDelayTimer.Stop();
			_regenStartDelayTimer.Start();
		}
		else
		{
			_hp = 0; // TODO: Die
		}
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("switch_gun"))
		{
			SwitchGun();
		}
    }

	public override void _PhysicsProcess(float delta)
	{
		_vel.y += Constants.Gravity;

		if (Input.IsActionPressed("left"))
		{
			_sprite.Play("run");
			_sprite.FlipH = true;
			_vel.x = -_speed;
            EquippedGun.SetDirection(-1);
        }
		else if (Input.IsActionPressed("right"))
		{
			_sprite.Play("run");
			_sprite.FlipH = false;
			_vel.x = _speed;
            EquippedGun.SetDirection(1);
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

	private void OnCanRegen()
	{
		_regenTickTimer.Start();
	}

	private void OnHeal()
	{
		const uint amount = 12;
		_hp += amount;
		if (_hp > 100)
		{
			_regenTickTimer.Stop();
			_hp = 100;
		}
	}
}
