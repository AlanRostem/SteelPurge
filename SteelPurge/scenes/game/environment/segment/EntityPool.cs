using Godot;
using System;
using Godot.Collections;

public class EntityPool : Node2D
{
	public WorldSegment ParentWorldSegment { get; private set; }

	private Dictionary<ulong, Dictionary<string, object>> _initialEntityDataPool =
		new Dictionary<ulong, Dictionary<string, object>>();

	private Dictionary<ulong, Dictionary<string, object>> _capturedEntityDataPool;

	public override void _Ready()
	{
		base._Ready();
		ParentWorldSegment = GetParent<WorldSegment>();
		_initialEntityDataPool = ExportEntityData();

		// GD.Print(JSON.Print(_initialEntityDataPool, "    "));
	}

	public T SpawnEntityDeferred<T>(PackedScene scene, Vector2 position) where T : KinematicEntity
	{
		var entity = (T) scene.Instance();
		entity.Position = position;
		CallDeferred("add_child", entity);
		return entity;
	}

	public T SpawnStaticEntityDeferred<T>(PackedScene scene, Vector2 position) where T : StaticEntity
	{
		var entity = (T) scene.Instance();
		entity.Position = position;
		CallDeferred("add_child", entity);
		return entity;
	}

	public void RemoveEntity(Node2D entity)
	{
		switch (entity)
		{
			case KinematicEntity kEntity:
				kEntity.OnRemoved();
				break;
			case StaticEntity sEntity:
				sEntity.OnRemoved();
				break;
		}

		entity.QueueFree();
	}

	public void CaptureCurrentEntityStates()
	{
		_capturedEntityDataPool = ExportEntityData();
	}

	// TODO: Optimize in the future. 
	public void ReturnToPreviouslyCapturedEntityStates()
	{
		ClearAllEntities();
		var seenScenes = new Dictionary<string, PackedScene>();
		foreach (var pair in _capturedEntityDataPool)
		{
			var scenePath = (string) pair.Value[nameof(Node2D.Filename)];
			if (!seenScenes.TryGetValue(scenePath, out var scene))
			{
				scene = seenScenes[scenePath] = GD.Load<PackedScene>(scenePath);
			}

			var entity = (Node2D) scene.Instance();
			CallDeferred("add_child", entity);
			switch (entity)
			{
				case KinematicEntity kEntity:
					kEntity.CallDeferred(nameof(KinematicEntity.FeedEntityData), pair.Value);
					break;
				case StaticEntity sEntity:
					sEntity.CallDeferred(nameof(StaticEntity.FeedEntityData), pair.Value);
					break;
			}
		}

		// GD.Print(JSON.Print(_capturedEntityDataPool, "    "));
		_capturedEntityDataPool.Clear();
	}

	public void ClearAllEntities()
	{
		foreach (Node2D entity in GetChildren())
			entity.CallDeferred("queue_free");
	}

	public void ResetEntityStates()
	{
		ClearAllEntities();
		var seenScenes = new Dictionary<string, PackedScene>();
		foreach (var pair in _initialEntityDataPool)
		{
			var scenePath = (string) pair.Value[nameof(Node2D.Filename)];
			if (!seenScenes.TryGetValue(scenePath, out var scene))
			{
				scene = seenScenes[scenePath] = GD.Load<PackedScene>(scenePath);
			}

			var entity = (Node2D) scene.Instance();
			switch (entity)
			{
				case KinematicEntity kEntity:
					kEntity.FeedEntityData(pair.Value);
					break;
				case StaticEntity sEntity:
					sEntity.FeedEntityData(pair.Value);
					break;
			}

			CallDeferred("add_child", entity);
		}
	}

	public Dictionary<ulong, Dictionary<string, object>> ExportEntityData()
	{
		var data = new Dictionary<ulong, Dictionary<string, object>>();
		foreach (Node2D entity in GetChildren())
		{
			switch (entity)
			{
				case KinematicEntity kEntity:
					var kData = kEntity.ExportEntityData();
					data.Add(kEntity.GetInstanceId(), kData);
					break;
				case StaticEntity sEntity:
					var sData = sEntity.ExportEntityData();
					data.Add(sEntity.GetInstanceId(), sData);
					break;
			}
		}

		return data;
	}
}
