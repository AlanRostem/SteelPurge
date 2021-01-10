using Godot;
using System;

public class Map : Node2D
{
	[Export] public uint MaxEnemyCountPerRound = 20;
	public uint CurrentEnemyCount = 20;
	public uint EnemiesOnMap = 0;
	public uint Round = 1;
	
	public override void _Ready()
	{
	}

	public override void _Process(float delta)
	{
		if (CurrentEnemyCount == 0)
		{
			Round++;
			CurrentEnemyCount = MaxEnemyCountPerRound;
			EnemiesOnMap = MaxEnemyCountPerRound;
		}
	}
}
