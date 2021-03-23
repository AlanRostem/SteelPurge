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
	public bool IsActive = false;

	[Signal]
	public delegate void TriggerDurationTimer();

	[Signal]
	public delegate void TriggerCoolDownTimer();

	public virtual void OnActivate()
	{

	}

	public virtual void OnUpdate()
	{

	}

	public virtual void OnEnd()
	{

	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		if (Input.IsActionJustPressed("tactical_ability"))
		{
			var fuels = GetWeapon().OwnerPlayer.PlayerInventory.OrdinanceFuels;
			if (!IsOnCoolDown && !IsActive && fuels[(int)FuelType] >= FuelRequirement)
			{
				IsActive = true;
				OnActivate();
				EmitSignal(nameof(TriggerDurationTimer));
				fuels[(int)FuelType] -= FuelRequirement;
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
		EmitSignal(nameof(TriggerCoolDownTimer));
		OnEnd();
	}
}
