using Godot;
using System;

public partial class game_over_menu : Node2D
{
	private PackedScene startMenu;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		startMenu = (PackedScene)ResourceLoader.Load("res://Resources/Menus/start_menu.tscn");
	}

	public void _on_timer_timeout() {
		GetTree().ChangeSceneToPacked(startMenu);
	}
}
