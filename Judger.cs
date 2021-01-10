using Godot;
using System;

public class Judger : Gun
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	public override void OnFire()
	{
		ScanHit();
		Sounds.PlaySound(Sounds.JudgerFireSound);

	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
