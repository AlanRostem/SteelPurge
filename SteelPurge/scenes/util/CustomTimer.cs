using Godot;
using System;

public class CustomTimer : Node
{
	[Signal]
	public delegate void Timeout();

	[Export] public float WaitTime = 1;
	[Export] public bool AutoStart = false;
	[Export] public bool OneShot = false;

	public float TimeElapsed => _currentTime;
	public float TimeLeft => WaitTime - _currentTime;

	private float _currentTime = 0;
	private bool _done = true;
	private bool _paused = false;

	public override void _Ready()
	{
		if (AutoStart)
			Start();
	}

	public override void _Process(float delta)
	{
		if (!_done && !_paused)
		{
			_currentTime += delta;
		}
		else
		{
			return;
		}

		if (_currentTime >= WaitTime)
		{
			_done = true;
			EmitSignal(nameof(Timeout));
			if (!OneShot)
				Start();
		}
	}

	public void Start(float at = 0)
	{
		_currentTime = at;
		_done = false;
		_paused = false;
	}

	public void Pause()
	{
		_paused = true;
	}

	public void Stop()
	{
		_done = true;
		_currentTime = 0;
	}
}
