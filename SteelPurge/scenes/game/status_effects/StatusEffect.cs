using Godot;
using System;

public class StatusEffect : Node2D
{
	[Export] public Entity.StatusEffectType Type = Entity.StatusEffectType.None;
	[Export] public float Duration = 1;
	public Entity Subject { get; private set; }

	public override void _Ready()
	{
		Subject = GetParent<Entity>();

		if (OS.IsDebugBuild() && Type == Entity.StatusEffectType.None)
			throw new Exception("StatusEffectType cannot have None type");
	}

	[Signal]
	public delegate void End(Entity subject);

	[Signal]
	public delegate void Reset();

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
