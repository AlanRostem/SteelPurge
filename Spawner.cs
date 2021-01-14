using Godot;
using System;

public class Spawner : StaticBody2D
{
	private static readonly PackedScene _enemyScene = GD.Load<PackedScene>("res://Enemy.tscn");
	private Player _playerRef;
	private Map _mapRef;
	private Timer _spawnTimer;

	public override void _Ready()
	{
		_mapRef = GetTree().Root.GetNode<Map>("Map");
		_playerRef = GetTree().Root.GetNode<Player>("Map/Player");
		_spawnTimer = GetNode<Timer>("SpawnTimer");
	}


	private void OnSpawn()
	{
		if (_mapRef.EnemiesOnMap < _mapRef.CurrentEnemyCount)
		{
			_mapRef.EnemiesOnMap++;
			var enemy = (Enemy) _enemyScene.Instance();
			enemy.GlobalPosition = GlobalPosition;
			enemy.Spawning = true;
			GetParent().AddChild(enemy);
		}
	}


	private void OnEnemyExitSpawner(object body)
	{
		if (body is Enemy enemy)
		{
			enemy.Spawning = false;
		}
	}

	private void OnStartSpawnTimer()
	{
		_spawnTimer.Start();
	}

	private void OnSpawnTimerDisabled()
	{
		_spawnTimer.Stop();
	}
}
