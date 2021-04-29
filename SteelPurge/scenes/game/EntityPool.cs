using Godot;
using System;

public class EntityPool : Node2D
{
	public static PackedScene ScrapScene =
		GD.Load<PackedScene>("res://scenes/game/entities/collectible/scrap/Scrap.tscn");


	// TODO: Separate what type of entity that can be spawned (kinematic, static, other)
	public T SpawnEntityDeferred<T>(PackedScene scene, Vector2 position) where T : Node2D
	{
		var entity = (T)scene.Instance();
		entity.Position = position;
		CallDeferred("add_child", entity);
		return entity;
	}
}
