using Godot;
using System;

public class StatusEffect : Node2D
{
	[Export] public KinematicEntity.StatusEffectType Type = KinematicEntity.StatusEffectType.None;
	[Export] public float Duration = 1;
	public KinematicEntity Subject { get; private set; }

	public override void _Ready()
	{
		Subject = GetParent<KinematicEntity>();

		if (OS.IsDebugBuild() && Type == KinematicEntity.StatusEffectType.None)
			throw new Exception("StatusEffectType cannot have None type");
	}

	[Signal]
	public delegate void Start(KinematicEntity subject);
	
	[Signal]
	public delegate void End(KinematicEntity subject);

	[Signal]
	public delegate void Reset();

	public override void _PhysicsProcess(float delta)
	{
		OnUpdate(delta);
	}

	public virtual void OnUpdate(float delta)
	{
		
	}

	public void EndEffect()
	{
		OnEnd();
	}

	private void OnEnd()
	{
		Subject.RemoveStatusEffect(Type);
		EmitSignal(nameof(End), Subject);
		QueueFree();
	}

	public void ResetTime()
	{
		EmitSignal(nameof(Reset));   
	}
}
