using Godot;
using System;
using Godot.Collections;

public class World : Node2D
{
	public Player PlayerNode { get; private set; }
	public EntityPool Entities { get; private set; }

	[Export] public Array<PackedScene> SegmentScenes = new Array<PackedScene>();
	
	public WorldSegment CurrentSegment;

	public override void _Ready()
	{
		PlayerNode = GetNode<Player>("Player");
		Entities = GetNode<EntityPool>("EntityPool");
	}
}
