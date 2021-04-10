using Godot;
using System;

public class PlayerHitBox : Area2D
{
	[Signal]
	public delegate void OnTakeDamage(uint damage, int knockBackDirection);

	public void TakeDamage(uint damage, int knockBackDirection = 0)
	{
		EmitSignal(nameof(OnTakeDamage), damage, knockBackDirection);
	}
}
