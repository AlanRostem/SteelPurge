using Godot;
using System;

public class Spawner : StaticEntity
{
	[Export] public PackedScene EntitySceneToSpawn;
	[Export] public float SpawnIntervalTime = 1f;

	private Timer _spawnTimer;

	public override void _Ready()
	{
		base._Ready();
		_spawnTimer = GetNode<Timer>("SpawnTimer");
		_spawnTimer.WaitTime = SpawnIntervalTime;
	}
	
	
	private void _OnScreenEntered()
	{
		_spawnTimer.Start();
	}
	
	private void _OnScreenExited()
	{
		_spawnTimer.Stop();
	}
	
	private void Spawn()
	{
		ParentWorld.Entities.SpawnEntityDeferred<KinematicEntity>(EntitySceneToSpawn, Position);
	}
}
