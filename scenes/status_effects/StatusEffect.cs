using Godot;
using System;

public class StatusEffect : Node2D
{
	[Export] public float Duration = 1;
	public Entity Subject { get; private set; }
	
	public override void _Ready()
	{
		Subject = GetParent<Entity>();
	}

	[Signal]
	public delegate void End(Entity subject);
}
