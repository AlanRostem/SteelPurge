using Godot;
using System;

public class ResourceAbility : WeaponAbility
{
	[Export] 
	public uint DrainPerTick = 1;
	
	[Export]
	public float LingerDuration = 0.6f;
	
	private bool _isActive = false;
	
	[Signal]
	public delegate void Linger();
	
	public override void _Process(float delta)
	{
		if (Input.IsActionPressed("tactical_ability"))
		{
			EmitSignal(nameof(Linger));
			if (!_isActive)
			{
				_isActive = true;
				OnActivate();
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
		_isActive = false;
		OnDeActivate();
	}
}
