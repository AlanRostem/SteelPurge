using Godot;
using System;

public class ExecutorSprite : Sprite
{
	private AR43Executor _executor;
	public override void _Ready()
	{
		_executor = GetParent<AR43Executor>();
	}

	public override void _Process(float delta)
	{
		FlipH = _executor.Direction < 0;
	}
}
