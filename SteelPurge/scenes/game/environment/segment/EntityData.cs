using Godot;
using Godot.Collections;

public class EntityData
{
	protected readonly Dictionary<string, object> Data = new Dictionary<string, object>();

	public Vector2 WorldPosition
	{
		set => Data["position"] = new Dictionary
		{
			["x"] = value.x,
			["y"] = value.y,
		};

		get
		{
			var pos = (Dictionary<string, object>) Data["position"];
			return new Vector2()
			{
				x = (float)pos["x"],
				y = (float)pos["y"],
			};
		}
	}

	public Vector2 Velocity
	{
		set => Data["velocity"] = new Dictionary
		{
			["x"] = value.x,
			["y"] = value.y,
		};
		
		get
		{
			var pos = (Dictionary<string, object>) Data["velocity"];
			return new Vector2()
			{
				x = (float)pos["x"],
				y = (float)pos["y"],
			};
		}
	}

	public string ScenePath
	{
		set => Data["scenePath"] = value;
		get => (string)Data["scenePath"];
	}

	public EntityData(KinematicEntity entity)
	{
		WorldPosition = entity.Position;
		ScenePath = entity.Filename;
		Velocity = entity.Velocity;
	}

	public EntityData(StaticEntity entity)
	{
		WorldPosition = entity.Position;
		ScenePath = entity.Filename;
	}

	public Dictionary<string, object> GetJson()
	{
		return Data;
	}
}
