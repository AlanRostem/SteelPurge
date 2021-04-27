using Godot;
using System;

public class Fabricator : Area2D
{
	private World _world;
	[Export] public bool IsDefaultSpawnPoint = false;
	public override void _Ready()
	{
		_world = GetParent<World>();
		if (IsDefaultSpawnPoint)
			_world.SpawnPoint = Position;
	}
	
	private void _OnPlayerEnter(object body)
	{
		// TODO: Consider more factors after the prototype
		_world.SpawnPoint = Position;
	}
}
