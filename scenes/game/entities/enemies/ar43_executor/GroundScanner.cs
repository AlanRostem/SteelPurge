using Godot;
using System;

public class GroundScanner : RayCast2D
{
	[Export] public float ScanRange = 20;
	private AR43Executor _executor;
	public override void _Ready()
	{
		_executor = GetParent<AR43Executor>();
	}

	public override void _Process(float delta)
	{
		if ((int) Position.x != _executor.Direction)
			Position = new Vector2(ScanRange * _executor.Direction, Position.y);
	}
}
