using Godot;
using System;

public class KnockBackEffect : StatusEffect
{
	public Vector2 KnockBackForce;
	public bool DisableEntityMovement = false;

	private void _OnStart(KinematicEntity subject)
	{
		
		if (Mathf.Abs(subject.VelocityX) < Mathf.Abs(KnockBackForce.x))
			subject.VelocityX = KnockBackForce.x;
		
		if (Mathf.Abs(subject.VelocityY) < Mathf.Abs(KnockBackForce.y))
			subject.VelocityY = KnockBackForce.y;

		if (!DisableEntityMovement) return;
		subject.CanMove = false;
	}

	public override void OnUpdate(float delta)
	{
		if (!DisableEntityMovement) return;
		Subject.VelocityX = Mathf.Lerp(Subject.VelocityX, 0, 0.1f);
	}

	private void _OnEnd(KinematicEntity subject)
	{
		if (!DisableEntityMovement) return;
		subject.CanMove = true;
	}    
}
