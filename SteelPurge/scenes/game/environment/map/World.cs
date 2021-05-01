using Godot;
using System;
using Godot.Collections;

public class World : Node2D
{
	public Player PlayerNode { get; private set; }

	public EntityPool Entities
	{
		get
		{
			return CurrentSegment.Entities;
		}
	}

	[Export] public Array<PackedScene> SegmentScenes = new Array<PackedScene>();
	
	public WorldSegment CurrentSegment { get; private set; }

	public void LoadSegment(int index)
	{
		CurrentSegment?.QueueFree();
		CurrentSegment = (WorldSegment)SegmentScenes[index].Instance();
		CallDeferred("add_child", CurrentSegment);
	}

	public override void _Ready()
	{
		PlayerNode = GetNode<Player>("Player");
		LoadSegment(0);
	}
}
