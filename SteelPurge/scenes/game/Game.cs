using Godot;
using Godot.Collections;

public class Game : Node2D
{
	private World _world;

	// private Dictionary<string, object> _saveData = new Dictionary<string, object>();
	private static readonly string MainMenuScenePath = "res://scenes/main_menu/MainMenu.tscn";

	public PlayerStats PlayerStats { get; private set; }

	private PauseObject _pauseObject = new PauseObject();
	private PauseMenu _pauseMenu;
	
	public override void _Ready()
	{
		base._Ready();
		_world = GetNode<World>("World");
		_pauseMenu = GetNode<PauseMenu>("CanvasLayer/PauseMenu");
		PlayerStats = new PlayerStats();
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			_pauseObject.PauseOrUnpause(GetTree());
			_pauseMenu.Visible = _pauseObject.IsPaused;
		}
	}
	
	private void _OnPauseMenuResume()
	{
		_pauseObject.TryToUnpause(GetTree());
	}
	
	private void _OnPauseMenuReturn()
	{
		_pauseObject.TryToUnpause(GetTree());
		GetTree().ChangeScene(MainMenuScenePath);
		QueueFree();
	}
}
