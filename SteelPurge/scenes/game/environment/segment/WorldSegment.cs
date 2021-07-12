using Godot;
using System;

public class WorldSegment : Node2D
{
	public Vector2 InitialSpawnPoint => GetNode<Position2D>("SpawnPoint").Position;

	public EntityPool Entities { get; private set; }

	public Node2D Environment { get; private set; }

	public World ParentWorld { get; private set; }

	private CustomTileMap _tileMap;

	[Export] public float TimeLimitSeconds = 40;

	private float _currentTimeLimit;
	private int _currentTimeLimitInt;

	private float _capturedTimeLimit;

	private Label _timeLabel;

	public override void _Ready()
	{
		Entities = GetNode<EntityPool>("EntityPool");
		Environment = GetNode<Node2D>("Environment");
		ParentWorld = GetParent<World>();
		_tileMap = GetNode<CustomTileMap>("Environment/CustomTileMap");
		var rect = _tileMap.GetUsedRect();
		rect.Position *= CustomTileMap.Size;
		rect.Size *= CustomTileMap.Size;
		ParentWorld.PlayerNode.SetCameraBounds(rect);
		ParentWorld.CurrentReSpawnPoint = new Vector2(InitialSpawnPoint);
		_timeLabel = GetNode<Label>("CanvasLayer/TimeLabel");
		ResetTimeLimit();
		ParentWorld.PlayerNode.Connect(nameof(Player.Died), this, nameof(ResetTimeLimit));
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		_currentTimeLimit -= delta;
		if (_currentTimeLimit <= 0)
		{
			ResetTimeLimit();
			ParentWorld.PlayerNode.Die();
		}

		var time = Mathf.RoundToInt(_currentTimeLimit);
		if (_currentTimeLimitInt == time) return;
		_currentTimeLimitInt = time;
		_timeLabel.Text = time + "s";
		if (_currentTimeLimit / TimeLimitSeconds < 0.15f)
		{
			if (_timeLabel.Modulate != Colors.Red)
				_timeLabel.Modulate = Colors.Red;
		}
		else if (_timeLabel.Modulate == Colors.Red)
			_timeLabel.Modulate = Colors.White;
	}

	private void _OnTransferAreaPlayerEntered(object body)
	{
		ParentWorld.SwitchToNextSegment();
	}

	public void ResetTimeLimit()
	{
		_currentTimeLimit = TimeLimitSeconds;
	}

	public void CaptureTimeLimit()
	{
		_capturedTimeLimit = _currentTimeLimit;
	}

	public void ReturnToCapturedTimeLimit()
	{
		_currentTimeLimit = _capturedTimeLimit;
		_timeLabel.Text = Mathf.RoundToInt(_currentTimeLimit) + "s";
	}
}
