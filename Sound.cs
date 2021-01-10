using Godot;
using System;

public class Sound : AudioStreamPlayer
{
	private void OnFinished()
	{
		QueueFree();
	}
}
