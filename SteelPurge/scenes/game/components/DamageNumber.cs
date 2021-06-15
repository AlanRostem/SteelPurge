using Godot;
using System;

public class DamageNumber : Label
{
	[Export] public float RiseSpeed = 10;

	private Timer _existenceTimer;
	private float _distanceRisen = 0;

	public uint Damage
	{
		set
		{
			Text = value.ToString();
			_existenceTimer.Start();
			RectPosition = new Vector2(RectPosition.x, RectPosition.y + _distanceRisen);
			_distanceRisen = 0;
		}
	}

	public override void _Ready()
	{
		_existenceTimer = GetNode<Timer>("ExistenceTimer");
	}

	public override void _Process(float delta)
	{
		RectPosition = new Vector2(RectPosition.x, RectPosition.y - RiseSpeed * delta);
		_distanceRisen -= RiseSpeed * delta;
	}
	
	private void _OnExistenceTimeout()
	{
		QueueFree();
	}
}


