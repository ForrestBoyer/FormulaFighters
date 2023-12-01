using Godot;
using System;

public partial class deck_menu : Node2D
{
	private PackedScene levelOne;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		levelOne = (PackedScene)ResourceLoader.Load("res://Resources/Levels/level_one.tscn");
	}

	// If play button pressed play game
	public void _on_back_pressed() {
		GetTree().ChangeSceneToPacked(levelOne);
	}
}
