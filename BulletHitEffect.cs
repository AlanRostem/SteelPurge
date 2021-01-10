using Godot;
using System;

public class BulletHitEffect : AnimatedSprite
{
	private void OnTimeout()
	{
		QueueFree();
	}
}
