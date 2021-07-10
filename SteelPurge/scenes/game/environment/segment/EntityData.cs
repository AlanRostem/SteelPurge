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

	public void SetAny<T>(string prop, T value)
	{
		_data[prop] = value;
	}

	public void SetTimer(string prop, CustomTimer timer)
	{
		SetDict(prop, new Dictionary<string, object>
		{
			[nameof(timer.TimeElapsed)] = timer.TimeElapsed,
			[nameof(timer.IsActive)] = timer.IsActive,
		});
	}

	public void ConfigureTimer(string prop, CustomTimer timer)
	{
		var timeElapsed = GetDictProp<float>(prop, nameof(CustomTimer.TimeElapsed));
		var isActive = GetDictProp<bool>(prop, nameof(CustomTimer.IsActive));
		if (isActive)
			timer.CallDeferred(nameof(CustomTimer.Start), timeElapsed);
		else
			timer.CallDeferred(nameof(CustomTimer.Stop));
	}

	public void SetDict(string prop, Dictionary<string, object> dict)
	{
		_data[prop] = dict;
	}

	public Dictionary GetDict(string prop)
	{
		return GetAny<Dictionary>(prop);
	}

	public T GetDictProp<T>(string prop, string propForDict)
	{
		var dict = (Dictionary) _data[prop];
		return (T) dict[propForDict];
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
