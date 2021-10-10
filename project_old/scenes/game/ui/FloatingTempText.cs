using Godot;
using System;

public class FloatingTempText : Label
{
	[Signal]
	public delegate void Disappear();
	
	[Export] public float RiseSpeed = 10;
	public Timer ExistenceTimer { get; private set; }

	public Vector2 Position
	{
		get => RectPosition + RectSize / 2;
		set => RectPosition = value - RectSize / 2;
	}

	public override void _Ready()
	{
		ExistenceTimer = GetNode<Timer>("ExistenceTimer");
	}

	public override void _Process(float delta)
	{
		RectPosition = new Vector2(RectPosition.x, RectPosition.y - RiseSpeed * delta);
	}
	
	private void _OnExistenceTimeout()
	{
		EmitSignal(nameof(Disappear));
		CallDeferred("queue_free");
	}
}
