using Godot;
using System;

public partial class start_menu : Node2D
{
	private PackedScene levelSelect;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		levelSelect = (PackedScene)ResourceLoader.Load("res://Resources/Menus/level_select_menu.tscn");
	}

	// If play button pressed play game
	public void _on_play_pressed() {
		GetTree().ChangeSceneToPacked(levelSelect);
	}

	// If quit button pressed quit game
	public void _on_quit_pressed() {
		GetTree().Quit();
	}
}
