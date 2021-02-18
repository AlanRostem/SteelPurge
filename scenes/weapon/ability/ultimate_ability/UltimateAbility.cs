using Godot;
using System;

public class UltimateAbility : WeaponAbility
{
	public static readonly uint MaxCharge = 100;
	public static readonly uint StandardDamageDivisor = 100;
	public float CurrentCharge = 0;

	/// Ultimate charge gained per X damage. Refer to how much the standard damage (X) is
	[Export] public float ChargePerDamage = 25;
	
	[Export] public float Duration = 1;
	public float CurrentDuration = 0;
	public bool IsActive = false;
	
	public override void _Ready()
	{
		base._Ready();
		GetWeapon().UltimateAbilityRef = this;
		GetWeapon().Connect(nameof(Weapon.DamageDealt), this, nameof(OnCharge));
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

	[Signal]
	public delegate void TriggerDurationTimer();
	
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("ultimate_ability") && CurrentCharge == MaxCharge)
		{
			EmitSignal(nameof(TriggerDurationTimer));
			IsActive = true;
			OnActivate();
		}

		if (IsActive)
			OnUpdate();
	}

	public void OnCharge(uint damage, VulnerableHitbox target)
	{
		if (IsActive) return;
		CurrentCharge += damage * ChargePerDamage / StandardDamageDivisor;
		if (CurrentCharge >= MaxCharge)
		{
			CurrentCharge = MaxCharge;
		}
	}
	
	private void _OnAbilityDone()
	{
		CurrentCharge = 0;
		IsActive = false;
		OnEnd();

	}
}
