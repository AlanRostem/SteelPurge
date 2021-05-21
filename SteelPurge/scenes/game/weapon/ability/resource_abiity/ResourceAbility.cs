using Godot;
using System;

public class ResourceAbility : WeaponAbility
{
	[Export] public uint DrainPerTick = 1;

	[Export] public float LingerDuration = 0.6f;

	[Export] public float DrainInterval = 0.1f;

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
		var type = (int) FuelType;
		var player = GetWeapon().OwnerPlayer;
		if (player is null) return;
		var fuels = player.PlayerInventory.OrdinanceFuels;

		if (IsActive)
		{
			OnUpdate();

			_currentDrainTime += delta;
			if (_currentDrainTime >= DrainInterval)
			{
				_currentDrainTime = 0;
				player.PlayerInventory.DrainFuel(FuelType, DrainPerTick);
				OnTick();
				_abilityBar.Value = fuels[type];
			}
		}
		
		if (fuels[type] < DrainPerTick && IsActive)
		{
			_LingerStopped();
			return;
		}

		var pressed = Input.IsActionPressed("tactical_ability") && GetWeapon().Equipped;

		if (pressed && fuels[type] > DrainPerTick)
		{
			EmitSignal(nameof(Linger));
			if (!IsActive)
			{
				IsActive = true;
				OnActivate();
				_abilityBar.MaxValue = fuels[type];
				_abilityBar.Value = fuels[type];
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
}
