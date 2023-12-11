using Godot;
using System;

public partial class GameOverMenu : Node2D
{
	public PackedScene mainMenu;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mainMenu = GD.Load<PackedScene>("res://Game/Menus/main_menu.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_timer_timeout()
	{
		GetTree().ChangeSceneToPacked(mainMenu);
	}
}
