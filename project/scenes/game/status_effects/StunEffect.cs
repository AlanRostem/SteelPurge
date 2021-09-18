using Godot;
using System;

public class StunEffect : StatusEffect
{
	private void _OnStart(KinematicEntity subject)
	{
		subject.CanMove = false;
		subject.VelocityX = 0;
		if (subject is Enemy enemy)
		{
			enemy.IsCurrentlyLethal = false;
			enemy.IsAiEnabled = false;
		}
	}

	private void _OnEnd(KinematicEntity subject)
	{
		subject.CanMove = true;
		if (subject is Enemy enemy)
		{
			enemy.IsCurrentlyLethal = true;
			enemy.IsAiEnabled = true;
		}
	}
}
