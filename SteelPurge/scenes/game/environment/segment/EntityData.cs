using Godot;
using Godot.Collections;

public sealed class EntityData
{
	private readonly Dictionary<string, object> _data;

	public EntityData()
	{
		_data = new Dictionary<string, object>();
	}
	
	public EntityData(Dictionary<string, object> data)
	{
		_data = data;
	}

	public Vector2 GetVector(string prop)
	{
		var v = (Dictionary) _data[prop];
		return new Vector2
		{
			x = (float) v["x"],
			y = (float) v["y"],
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

	public S GetAny<S>(string prop)
	{
		return (S) _data[prop];
	}

	public Dictionary<string, object> GetJson()
	{
		return _data;
	}
}
