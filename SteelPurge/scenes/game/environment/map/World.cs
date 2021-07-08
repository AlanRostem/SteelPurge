using Godot;
using System;
using Godot.Collections;

public class World : Node2D
{
	public Player PlayerNode { get; private set; }


	public Vector2 CurrentReSpawnPoint;

	[Export] public Array<PackedScene> SegmentScenes = new Array<PackedScene>();
	
	public WorldSegment CurrentSegment { get; private set; }

	private int _currentSegmentIndex = 0;
	
	public void LoadSegment(int index)
	{
		CurrentSegment?.QueueFree();
		CurrentSegment = (WorldSegment)SegmentScenes[index].Instance();
		CallDeferred("add_child", CurrentSegment);
	}

	public override void _Ready()
	{
		PlayerNode = GetNode<Player>("Player");
		CreateFirstSegment();
	}

	public void CreateFirstSegment()
	{
		LoadSegment(0);
		PlayerNode.Position = CurrentSegment.InitialSpawnPoint;
	}

	public void SwitchToNextSegment()
	{
		PlayerNode.PlayerInventory?.EquippedWeapon.OnSwap();
		LoadSegment(++_currentSegmentIndex);
		PlayerNode.Position = CurrentSegment.InitialSpawnPoint;
	}

	public void SetPaused(bool paused)
	{
		GetTree().Paused = paused;
	}
	
	private void _OnPlayerDied()
	{
		LoadSegment(_currentSegmentIndex);
	}
}
