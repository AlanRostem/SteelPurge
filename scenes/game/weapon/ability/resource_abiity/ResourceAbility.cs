using Godot;
using System;

public class ResourceAbility : WeaponAbility
{
	[Export] 
	public uint DrainPerTick = 1;
	
	[Export]
	public float LingerDuration = 0.6f;
	
	[Export]
	public float DrainInterval = 0.4f; 
	
	private float _currentDrainTime = 0;
	private bool _isActive = false;
	
	[Signal]
	public delegate void Linger();
	
	public override void _Process(float delta)
	{
		if (Input.IsActionPressed("tactical_ability"))
		{
			EmitSignal(nameof(Linger));
			// TODO: Drain the resource
			if (!_isActive)
			{
				_isActive = true;
				OnActivate();
			}
		}
		
		if (_isActive)
		{
			var type = (int)FuelType;
			var player = GetWeapon().OwnerPlayer;
			var fuels = player.PlayerInventory.OrdinanceFuels;
			if (fuels[type] >= DrainPerTick)
			{
				if (_currentDrainTime >= DrainInterval) 
				{			
					_currentDrainTime = 0;
					fuels[type] -= DrainPerTick;
					player.KnowInventoryOrdinanceFuelCount(fuels[type], FuelType);
				}
				_currentDrainTime += delta;
			}
			else
			{	
				_LingerStopped();
			}
		}
	}
	
	public virtual void OnActivate()
	{
		
	}
	
	public virtual void OnDeActivate()
	{
		
	}
	
	private void _LingerStopped()
	{
		_currentDrainTime = 0;
		_isActive = false;
		OnDeActivate();
	}
}
