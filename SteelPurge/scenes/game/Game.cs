using Godot;
using Godot.Collections;

public class Game : Node2D
{
	private World _world;

	// private Dictionary<string, object> _saveData = new Dictionary<string, object>();

	public PlayerStats PlayerStats { get; private set; }

	private bool _isPaused = false;
	private bool _isJustUnPaused = false;

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
			if (_isPaused)
			{
				GetTree().Paused = false;
				_isPaused = false;
				_isJustUnPaused = true;
			}
			
			if (!GetTree().Paused && !_isJustUnPaused)
			{
				GetTree().Paused = true;
				_isPaused = true;
			}

			if (_isJustUnPaused)
				_isJustUnPaused = false;
		}
	}
}
