using Godot;
using System;

public partial class win_menu : Node2D
{
	private PackedScene rewardMenu;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rewardMenu = (PackedScene)ResourceLoader.Load("res://Resources/Menus/reward_menu.tscn");
	}

	public void _on_timer_timeout() {
		GetTree().ChangeSceneToPacked(rewardMenu);
	}
}
