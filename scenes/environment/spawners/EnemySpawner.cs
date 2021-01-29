using Godot;
using System;

public class EnemySpawner : Node2D
{
	[Export(PropertyHint.ResourceType)] public PackedScene EnemyScene;
	[Export] public float SpawnDelay = 1;

	private Map _parenMap;
	public override void _Ready()
	{
		_parenMap = GetParent<Map>();
	}
	
	private void _OnSpawn()
	{
		if (_parenMap.EnemiesOnMap < _parenMap.CurrentExpectedEnemies)
		{
			_parenMap.EnemiesOnMap++;
			var enemy = (Enemy)EnemyScene.Instance();
			_parenMap.AddChild(enemy);
			enemy.Position = Position;
		}
	}
}
