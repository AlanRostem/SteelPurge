using Godot;
using System;

public class BurnEffect : StatusEffect
{
	[Export] public float DamageInterval = 0.8f;
	[Export] public uint DamagePerTick = 5;

	private float _currentDamageTime = 0;

	public override void _Process(float delta)
	{
		_currentDamageTime += delta;
		if (_currentDamageTime >= DamageInterval)
		{
			_currentDamageTime = 0;
			Subject.TakeDamage(DamagePerTick, Vector2.Zero);
		}
	}
}
