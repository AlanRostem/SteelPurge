using Godot;
using System;

public class RoundLabel : Label
{
	Map _mapRef;
	public override void _Ready()
	{
		_mapRef = GetTree().Root.GetNode<Map>("Map");
	}

	public override void _Process(float delta)
	{
	  	Text = _mapRef.Round.ToString();
	}
}
