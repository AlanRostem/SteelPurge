using Godot;
using System;

public class EntityPool : Node2D
{
	public static PackedScene ScrapScene =
		GD.Load<PackedScene>("res://scenes/game/entities/collectible/scrap/Scrap.tscn");


	public WorldSegment ParentWorldSegment { get; private set;  }

	public override void _Ready()
	{
		base._Ready();
		ParentWorldSegment = GetParent<WorldSegment>();
	}

	// TODO: Separate what type of entity that can be spawned (kinematic, static, other)
	public T SpawnEntityDeferred<T>(PackedScene scene, Vector2 position) where T : KinematicEntity
	{
		var entity = (T)scene.Instance();
		entity.Position = position;
		CallDeferred("add_child", entity);
		return entity;
	}	
	
	public T SpawnStaticEntityDeferred<T>(PackedScene scene, Vector2 position) where T : StaticEntity
	{
		var entity = (T)scene.Instance();
		entity.Position = position;
		CallDeferred("add_child", entity);
		return entity;
	}
}
