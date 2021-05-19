using Godot;
using Godot.Collections;

public class Game : Node2D
{
	private World _world;

	// private Dictionary<string, object> _saveData = new Dictionary<string, object>();

	public PlayerStats PlayerStats { get; private set; }

	private PauseObject _pauseObject = new PauseObject();
	
	public override void _Ready()
	{
		base._Ready();
		_world = GetNode<World>("World");
		PlayerStats = new PlayerStats();
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			_pauseObject.PauseOrUnpause(GetTree());
		}
	}
}
