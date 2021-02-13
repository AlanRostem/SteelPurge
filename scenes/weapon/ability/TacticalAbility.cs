using Godot;
using System;

public class TacticalAbility : WeaponAbility
{
    [Export] public Texture Icon;
    [Export] public float CoolDown;

    private bool _isOnCoolDown = false;

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Input.IsActionJustPressed("tactical_ability"))
        {
            if (!_isOnCoolDown)
            {

            }
            else
            {
                // TODO: Play sound and flash red on icon
            }
        }
    }
}
