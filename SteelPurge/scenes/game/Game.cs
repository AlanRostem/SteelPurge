using Godot;
using Godot.Collections;

public class Game : Node2D
{
	private World _world;

	// private Dictionary<string, object> _saveData = new Dictionary<string, object>();

	[Signal]
	private delegate void Paused();
	
	[Signal]
	private delegate void OpenInventory();
	
	public PlayerStats PlayerStats { get; private set; }

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
			GetTree().Paused = !GetTree().Paused;
			EmitSignal(nameof(Paused));
		}
		
		if (Input.IsActionJustPressed("inventory"))
		{
			GetTree().Paused = !GetTree().Paused;
			EmitSignal(nameof(OpenInventory));
		}
	}
}
