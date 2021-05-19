using Godot;

public class InventoryUI : Control
{
	public override void _Ready()
	{
		Visible = false;
	}

	private PauseObject _pauseObject = new PauseObject();

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("inventory"))
		{
			_pauseObject.PauseOrUnpause(GetTree());
			Visible = _pauseObject.IsPaused;
		}
	}

	private void _OnOpen()
	{
		Visible = !Visible;
	}
}
