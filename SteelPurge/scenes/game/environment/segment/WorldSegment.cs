using Godot;
using System;

public class WorldSegment : Node2D
{
	public Vector2 ReSpawnPoint
	{
		get
		{
			return GetNode<Fabricator>("Environment/Fabricator").Position;
		}
	}

	public Vector2 InitialSpawnPoint
	{
		get
		{
			return GetNode<Position2D>("SpawnPoint").Position;
		}
	}

	public EntityPool Entities { get; private set; }

	private World _parentWorld;
	private CustomTileMap _tileMap;
	
	public override void _Ready()
	{
		Entities = GetNode<EntityPool>("EntityPool");
		_parentWorld = GetParent<World>();
		_tileMap = GetNode<CustomTileMap>("Environment/CustomTileMap");
		var rect = _tileMap.GetUsedRect();
		rect.Position *= CustomTileMap.Size;
		rect.Size *= CustomTileMap.Size;
		_parentWorld.PlayerNode.SetCameraBounds(rect);
	}
	
	private void _OnTransferAreaPlayerEntered(object body)
	{
		_parentWorld.SwitchToNextSegment();
	}
}
