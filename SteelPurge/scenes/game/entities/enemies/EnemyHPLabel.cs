using Godot;
using System;

public class EnemyHPLabel : Label
{
	private void _OnEnemyHealthChanged(uint health)
	{
		Text = health.ToString();
	}
}
