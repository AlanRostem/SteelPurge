using Godot;
using System;

public class TacticalAbility : WeaponAbility
{
	[Export] public uint FuelRequirement = 10;
	[Export] public float CoolDown = 6;
	[Export] public float Duration = 1;
	[Export] public bool ShowBarWhenInUse = true;
	public float CurrentDuration => _durationTimer.TimeLeft;
	public float CurrentCoolDown => _cooldownTimer.TimeLeft;

	public bool IsOnCoolDown = false;

	private Timer _cooldownTimer;
	private Timer _durationTimer;
	private TextureProgress _abilityBar;

	[Signal]
	public delegate void TriggerDurationTimer();

	[Signal]
	public delegate void TriggerCoolDownTimer();

	public override void _Ready()
	{
		base._Ready();
		_cooldownTimer = GetNode<Timer>("CoolDownTimer");
		_durationTimer = GetNode<Timer>("DurationTimer");
		_abilityBar = GetNode<TextureProgress>("AbilityBar");
		GetWeapon().TacticalEnhancement = this;
		_abilityBar.Visible = false;
	}

	public virtual void OnActivate()
	{
	}

	public virtual void OnUpdate()
	{
	}

	public virtual void OnEnd()
	{
	}

	public override void DeActivate()
	{
		if (!ShowBarWhenInUse)
			_abilityBar.Visible = true;
		IsActive = false;
		IsOnCoolDown = true;
		_cooldownTimer.Start();
		_durationTimer.Stop();
		_abilityBar.MaxValue = CoolDown * 1000;
		_abilityBar.Value = 0;
		OnEnd();
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		if (Input.IsActionJustPressed("tactical_ability") && GetWeapon().Equipped)
		{
			var equippedWeaponEnum = GetWeapon().OwnerPlayer.PlayerInventory.EquippedWeaponEnum;
			var count = GetWeapon().OwnerPlayer.PlayerInventory.GetOrdinanceFuel(equippedWeaponEnum);
			if (!IsOnCoolDown && !IsActive && count >= FuelRequirement)
			{
				IsActive = true;
				OnActivate();
				_durationTimer.Start();
				GetWeapon().OwnerPlayer.PlayerInventory.DecreaseOrdinanceFuel(equippedWeaponEnum, FuelRequirement);
				if (ShowBarWhenInUse)
					_abilityBar.Visible = true;
				_abilityBar.MaxValue = Duration * 1000;
				_abilityBar.Value = Duration * 1000;
			}
			else
			{
				// TODO: Play sound and flash red on icon
			}
		}

		if (IsActive)
		{
			OnUpdate();
			_abilityBar.Value -= delta * 1000;
		}
		else if (IsOnCoolDown)
		{
			_abilityBar.Value += delta * 1000;
		}
	}

	private void _OnCoolDownFinished()
	{
		IsOnCoolDown = false;
		_abilityBar.Visible = false;
	}

	private void _OnStartCoolDown()
	{
		if (!ShowBarWhenInUse)
			_abilityBar.Visible = true;
		IsActive = false;
		IsOnCoolDown = true;
		_cooldownTimer.Start();
		_abilityBar.MaxValue = CoolDown * 1000;
		_abilityBar.Value = 0;
		OnEnd();
	}

	public override void OnWeaponSwapped()
	{
		_abilityBar.Visible = false;
	}

	public override void OnSwitchTo()
	{
		_OnStartCoolDown();
	}
}
