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

	private bool _isStartingNewRound = false;

	public override void _Ready()
	{
		var main = (Main) GetParent();
		main.CurrentMap = this;
		// TODO: Remove this temporary solution 

		EnemiesOnMap = 0;
		CurrentExpectedEnemies = MaxEnemiesPerRound;
	}

	public override void _Process(float delta)
	{
		if (CurrentExpectedEnemies == 0)
		{
			BeginNewRound();
		}
	}

	public void BeginNewRound()
	{
		Round++;
		MaxEnemiesPerRound += EnemyCountIncreasePerRound;
		EnemiesOnMap = 0;
		CurrentExpectedEnemies = MaxEnemiesPerRound;
	}
}
