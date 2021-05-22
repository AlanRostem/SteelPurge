using Godot;
using System;

public class InteractionIndicator : Area2D
{
	public delegate void Interacted(Player player);
	
	private Label _label;
	private bool _isPlayerHere = false;
	private Player _player = null;

	public override void _Ready()
	{
		_label = GetNode<Label>("PixelTextLabel");
		_label.Visible = false;
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("interact") && _isPlayerHere)
		{
			EmitSignal(nameof(Interacted), _player);
		}
	}

	private void _OnPlayerEntered(Player player)
	{
		if (_player is null)
			_player = player;
		_isPlayerHere = true;
		_label.Visible = true;
	}
	
	private void _OnPlayerLeave(Player body)
	{
		_isPlayerHere = false;
		_label.Visible = false;
	}
}
