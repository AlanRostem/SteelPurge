using Godot;
using System;

public class TacticalAbility : WeaponAbility
{
	[Export] public uint FuelRequirement = 10;
	[Export] public float CoolDown = 6;
	[Export] public float Duration = 1;
	public float CurrentDuration = 0;
	public float CurrentCoolDown = 0;
	
	public bool IsOnCoolDown = false;

	private Timer _cooldownTimer;
	private Timer _durationTimer;

	[Signal]
	public delegate void TriggerDurationTimer();

	[Signal]
	public delegate void TriggerCoolDownTimer();

	public override void _Ready()
	{
		base._Ready();
		_cooldownTimer = GetNode<Timer>("CoolDownTimer");
		_durationTimer = GetNode<Timer>("DurationTimer");
		GetWeapon().TacticalEnhancement = this;
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
		IsActive = false;
		IsOnCoolDown = true;
		_cooldownTimer.Start();
		_durationTimer.Stop();
		OnEnd();
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		if (Input.IsActionJustPressed("tactical_ability") && GetWeapon().Equipped)
		{
			var fuels = GetWeapon().OwnerPlayer.PlayerInventory.OrdinanceFuels;
			var count = fuels[(int) FuelType];
			if (!IsOnCoolDown && !IsActive && count >= FuelRequirement)
			{
				IsActive = true;
				OnActivate();
				_durationTimer.Start();
				GetWeapon().OwnerPlayer.PlayerInventory.DrainFuel(FuelType, FuelRequirement);
			}
			else
			{
				// TODO: Play sound and flash red on icon
			}
		}
		
		if (IsActive)
			OnUpdate();
	}
	
	private void _OnCoolDownFinished()
	{
		IsOnCoolDown = false;
	}
	
	private void _OnStartCoolDown()
	{
		IsActive = false;
		IsOnCoolDown = true;
		_cooldownTimer.Start();
		OnEnd();
	}
}
