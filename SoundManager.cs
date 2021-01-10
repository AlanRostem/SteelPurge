using Godot;
using System;

public class SoundManager : Node2D
{
	private static readonly PackedScene SoundScene = GD.Load<PackedScene>("res://Sound.tscn");
	[Export(PropertyHint.ResourceType)] public AudioStream MG27FireSound;
	[Export(PropertyHint.ResourceType)] public AudioStream ReloadStartSound;
	[Export(PropertyHint.ResourceType)] public AudioStream ReloadEndSound;
	
	
	public void PlaySound(AudioStream stream)
	{
		var player = (Sound) SoundScene.Instance();
		player.Stream = stream;
		AddChild(player);
		player.Play();
	}
}
