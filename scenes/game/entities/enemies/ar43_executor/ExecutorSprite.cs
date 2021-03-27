using Godot;
using System;

public class ExecutorSprite : Sprite
{
	private AR43Executor _executor;
	private int _direction = -1;
	public override void _Ready()
	{
		_executor = GetParent<AR43Executor>();
	}

	public override void _Process(float delta)
	{
		if (_direction != _executor.Direction)
		{
			FlipH = (_direction = _executor.Direction) < 0;
		}
	}
}
