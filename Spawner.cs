using Godot;
using System;

public class Spawner : StaticBody2D
{
	private static PackedScene _enemyScene = GD.Load<PackedScene>("res://Enemy.tscn");
	[Export] public uint SpawnCount;


	public override void _Ready()
	{
	}


	private void OnSpawn()
	{
		if (SpawnCount > 0)
		{
			SpawnCount--;
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
}
