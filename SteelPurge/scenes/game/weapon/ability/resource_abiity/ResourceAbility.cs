using Godot;
using System;

public class ResourceAbility : WeaponAbility
{
	[Export] public uint DrainPerTick = 1;

	[Export] public float LingerDuration = 0.6f;

	[Export] public float DrainInterval = 0.1f;

	private float _currentDrainTime = 0;

	[Signal]
	public delegate void Linger();

	public override void _Ready()
	{
		base._Ready();
		GetWeapon().TacticalEnhancement = this;
	}

	public override void _Process(float delta)
	{
		var type = (int) FuelType;
		var player = GetWeapon().OwnerPlayer;
		var fuels = player.PlayerInventory.OrdinanceFuels;

		if (IsActive)
		{
			OnUpdate();

			_currentDrainTime += delta;
			if (_currentDrainTime >= DrainInterval)
			{
				_currentDrainTime = 0;
				fuels[type] -= DrainPerTick;
				player.KnowInventoryOrdinanceFuelCount(fuels[type], FuelType);
				OnTick();
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
	}
}
