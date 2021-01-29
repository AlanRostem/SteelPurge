using Godot;
using System;

public class SpawnTimer : Timer
{
	private EnemySpawner _parent;
	
	public override void _Ready()
	{
		_parent = GetParent<EnemySpawner>();
		WaitTime = _parent.SpawnDelay;
	}
}
