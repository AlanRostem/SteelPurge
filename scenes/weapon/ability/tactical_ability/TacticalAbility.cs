using Godot;
using System;

public class TacticalAbility : WeaponAbility
{
	[Export] public Texture Icon;
	[Export] public float CoolDown = 6;
	[Export] public float Duration = 1;

	private bool _isOnCoolDown = false;
	private bool _isActive = false;

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

	public virtual void OnDeActivate()
	{

	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		if (Input.IsActionJustPressed("tactical_ability"))
		{
			if (!_isOnCoolDown && !_isActive)
			{
				_isActive = true;
				OnActivate();
				EmitSignal(nameof(TriggerDurationTimer));
			}
			else
			{
				// TODO: Play sound and flash red on icon
			}
		}
	}
	
	private void _OnCoolDownFinished()
	{
		_isOnCoolDown = false;
	}
	
	private void _OnStartCoolDown()
	{
		_isActive = false;
		_isOnCoolDown = true;
		EmitSignal(nameof(TriggerCoolDownTimer));
	}
}
