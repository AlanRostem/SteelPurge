using Godot;
using System;
using Godot.Collections;

public class World : Node2D
{
	public Player PlayerNode { get; private set; }

	public Vector2 CurrentReSpawnPoint;


	[Export] public Array<PackedScene> SegmentScenes = new Array<PackedScene>();

	public WorldSegment CurrentSegment { get; private set; }

	private int _currentSegmentIndex = 0;

	[Export] public float TimeLimitSeconds = 90;

	private float _currentTimeLimit;
	private int _currentTimeLimitInt;

	private float _capturedTimeLimit;
	private bool _timeLimitPaused;

	private Label _timeLabel;

	public void LoadSegment(int index)
	{
		CurrentSegment?.QueueFree();
		CurrentSegment = (WorldSegment) SegmentScenes[index].Instance();
		CallDeferred("add_child", CurrentSegment);
		_timeLabel = GetNode<Label>("CanvasLayer/TimeLabel");
	}

	public override void _Ready()
	{
		PlayerNode = GetNode<Player>("Player");
		CreateFirstSegment();
		PlayerNode.Connect(nameof(Player.Died), this, nameof(ResetTimeLimit));
	}

	public void CreateFirstSegment()
	{
		ResetTimeLimit();
		LoadSegment(0);
		PlayerNode.Position = CurrentSegment.InitialSpawnPoint;
		CurrentReSpawnPoint = new Vector2(CurrentSegment.InitialSpawnPoint);
	}

	public void SwitchToNextSegment()
	{
		PauseTimeLimit();
		PlayerNode.PlayerInventory?.EquippedWeapon.OnSwap();
		PlayerNode.ClearChronoVector();
		LoadSegment(++_currentSegmentIndex);
		PlayerNode.Position = new Vector2(CurrentSegment.InitialSpawnPoint);
		UnPauseTimeLimit();
	}

	public void SetPaused(bool paused)
	{
		GetTree().Paused = paused;
	}

	private void _OnPlayerDied()
	{
		if (_currentSegmentIndex == 0)
		{
			CurrentSegment.Entities.ResetEntityStates();
			PlayerNode.Position = new Vector2(CurrentSegment.InitialSpawnPoint);
		}
		else
		{
			CreateFirstSegment();
			_currentSegmentIndex = 0;
		}
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		if (!_timeLimitPaused)
			_currentTimeLimit -= delta;
		if (_currentTimeLimit <= 0)
		{
			ResetTimeLimit();
			PlayerNode.Die();
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

	public void PauseTimeLimit()
	{
		_timeLimitPaused = true;
	}

	public void UnPauseTimeLimit()
	{
		_timeLimitPaused = false;
	}
}
