using Godot;
using System;

public class InteractionIndicator : Area2D
{
	[Signal]
	public delegate void Interacted(Player player);

	[Export] public bool DisableOnInteract = false;

	private Label _label;
	private bool _isPlayerHere = false;
	private Player _player = null;
	private bool _hasInteracted = false;

	public override void _Ready()
	{
		_label = GetNode<Label>("PixelTextLabel");
		_label.Visible = false;
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("interact") && _isPlayerHere)
		{
			if (DisableOnInteract)
			{
				if (!_hasInteracted)
				{
					_hasInteracted = true;
					_label.Visible = false;
				}
				else return;
			}

			EmitSignal(nameof(Interacted), _player);
		}
	}

	private void _OnPlayerEntered(Player player)
	{
		if (_player is null)
			_player = player;
		_isPlayerHere = true;
		if (!_hasInteracted)
			_label.Visible = true;
	}

	private void _OnPlayerLeave(Player body)
	{
		_isPlayerHere = false;
		_label.Visible = false;
	}
}
