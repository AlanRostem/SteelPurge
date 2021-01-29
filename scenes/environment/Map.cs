using Godot;
using System;

public class Map : Node2D
{
	public Player PlayerRef;

	[Export] public uint MaxEnemiesPerRound = 20;
	[Export] public uint EnemyCountIncreasePerRound = 5;
	public uint EnemiesOnMap;
	public uint CurrentExpectedEnemies;
	public uint Round = 1;

	public override void _Ready()
	{
		var main = (Main) GetParent();
		main.CurrentMap = this;
		// TODO: Remove this temporary solution 

		EnemiesOnMap = 0;
		CurrentExpectedEnemies = MaxEnemiesPerRound;
	}

	public void BeginNewRound()
	{
		Round++;
		MaxEnemiesPerRound += EnemyCountIncreasePerRound;
		EnemiesOnMap = 0;
		CurrentExpectedEnemies = MaxEnemiesPerRound;
	}
}
