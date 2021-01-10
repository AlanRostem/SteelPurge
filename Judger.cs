using Godot;
using System;

public class Judger : Gun
{
	public override void OnFire()
	{
		ScanHit();
		Sounds.PlaySound(Sounds.JudgerFireSound);

	}
}
