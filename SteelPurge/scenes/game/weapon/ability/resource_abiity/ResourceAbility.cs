using Godot;
using System;

public class ResourceAbility : WeaponAbility
{
	[Export] public float LingerDuration = 0.6f;

	[Export] public float DrainInterval = 0.1f;
	[Export] public uint AmmoDrainPerTick = 1;

	private float _currentDrainTime = 0;

	private TextureProgress _abilityBar;

	[Signal]
	public delegate void Linger();

	public override void _Ready()
	{
		base._Ready();
		GetWeapon().TacticalEnhancement = this;
		_abilityBar = GetNode<TextureProgress>("AbilityBar");
		_abilityBar.Visible = false;
	}

	public override void _Process(float delta)
	{
		var player = GetWeapon().OwnerPlayer;
		if (player is null) return;
		if (IsActive)
		{
			OnUpdate();

			_currentDrainTime += delta;
			if (_currentDrainTime >= DrainInterval)
			{
				_currentDrainTime = 0;
				GetWeapon().RemoveAmmo(AmmoDrainPerTick);
				OnTick();
				_abilityBar.Value = GetWeapon().Ammo;
			}
		}
		
		if (GetWeapon().Ammo < AmmoDrainPerTick && IsActive)
		{
			_LingerStopped();
			return;
		}

		var pressed = Input.IsActionPressed("tactical_ability") && GetWeapon().Equipped;

		if (pressed && GetWeapon().Ammo >= AmmoDrainPerTick)
		{
			EmitSignal(nameof(Linger));
			if (!IsActive)
			{
				IsActive = true;
				OnActivate();
				_abilityBar.MaxValue = GetWeapon().MaxAmmo;
				_abilityBar.Value = GetWeapon().Ammo;
				_abilityBar.Visible = true;
			}
		}
	}

	public virtual void OnActivate()
	{
	}

	public virtual void OnTick()
	{
	}

	public virtual void OnUpdate()
	{
	}

	public virtual void OnDeActivate()
	{
	}

	private void _LingerStopped()
	{
		_currentDrainTime = 0;
		IsActive = false;
		OnDeActivate();
		_abilityBar.Visible = false;
	}

	public override void OnWeaponSwapped()
	{
		_abilityBar.Visible = false;
	}
}
