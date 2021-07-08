using Godot;
using Godot.Collections;

public class EntityData
{
	private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

	public Vector2 WorldPosition
	{
		set => SetVector("position", value);
		get => GetVector("position");
	}

	public Vector2 Velocity
	{
		set => SetVector("velocity", value);
		get => GetVector("velocity");
	}

	public string ScenePath
	{
		set => SetAny("scenePath", value);
		get => GetAny<string>("scenePath");
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
	
	public Vector2 GetVector(string prop)
	{
		var v = (Dictionary<string, object>) _data[prop];
		return new Vector2
		{
			x = (float)v["x"],
			y = (float)v["y"],
		};
	}

	public void SetVector(string prop, Vector2 value)
	{
		_data[prop] = new Dictionary
		{
			["x"] = value.x,
			["y"] = value.y,
		};
	}

	public void SetAny(string prop, object value)
	{
		_data[prop] = value;
	}
	
	public T GetAny<T>(string prop)
	{
		return (T) _data[prop];
	}

	public Dictionary<string, object> GetJson()
	{
		return _data;
	}
}
