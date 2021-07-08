using Godot;
using System;
using Godot.Collections;

public class EntityPool : Node2D
{
	public WorldSegment ParentWorldSegment { get; private set;  }
	
	public override void _Ready()
	{
		base._Ready();
		ParentWorldSegment = GetParent<WorldSegment>();
	}

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

	public Array<Dictionary<string, object>> ExportEntityData()
	{
		var data = new Array<Dictionary<string, object>>();
		foreach (Node2D entity in GetChildren())
		{
			switch (entity)
			{
				case KinematicEntity kEntity:
					var kData = kEntity.ExportEntityData();
					var kJson = kData is null ? new EntityData(kEntity).GetJson() : kData.GetJson();
					data.Add(kJson);
					break;
				case StaticEntity sEntity:
					var sData = sEntity.ExportEntityData();
					var sJson = sData is null ? new EntityData(sEntity).GetJson() : sData.GetJson();
					data.Add(sJson);
					break;
			}
		}
		return data;
	}
}
