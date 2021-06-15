using Godot;
using System;

public class DamageNumber : Label
{
	[Signal]
	public delegate void Disappear();
	
	[Export] public float RiseSpeed = 10;

	private Timer _existenceTimer;
	private float _distanceRisen = 0;
	private uint _damage = 0;

	public uint Damage
	{
		get => _damage;
		set
		{
			Text = value.ToString();
			if (_existenceTimer is null)
				_existenceTimer = GetNode<Timer>("ExistenceTimer");
			_existenceTimer.CallDeferred("start");
			RectPosition = new Vector2(RectPosition.x, RectPosition.y - _distanceRisen);
			_damage = value;
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
		EmitSignal(nameof(Disappear));
		CallDeferred("queue_free");
	}
}


