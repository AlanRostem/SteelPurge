using Godot;
using System;

public class Spawner : StaticBody2D
{
    private static PackedScene _enemyScene = GD.Load<PackedScene>("res://Enemy.tscn");
    [Export] public uint SpawnCount;
    private Player _playerRef;
    private Map _mapRef;

    public override void _Ready()
    {
        _mapRef = GetTree().Root.GetNode<Map>("Map");
        _playerRef = GetTree().Root.GetNode<Player>("Map/Player");
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