using System;
using Godot;

class Constants
{
    public static readonly float Gravity = 20;
    public static readonly Vector2 Up = new Vector2(0, -1);
    public static readonly float AiDistance = Mathf.Sqrt(320 * 320 + 180 * 180) / 2;
}