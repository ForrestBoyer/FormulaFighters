using Godot;
using System;

public partial class level_select_menu : Node2D
{
	private PackedScene levelOne;
	private PackedScene inventory;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		levelOne = (PackedScene)ResourceLoader.Load("res://Resources/Levels/level_one.tscn");
		inventory = (PackedScene)ResourceLoader.Load("res://Resources/Menus/inventory_menu.tscn");
	}

	public void _on_area_2d_input_event(Viewport view, InputEvent @event, int shape_idx) {
		if (@event is InputEventMouseButton && @event.IsPressed()) {
            GetTree().ChangeSceneToPacked(levelOne);
        }
	}

	public void _on_inventory_input_event(Viewport view, InputEvent @event, int shape_idx) {
		if (@event is InputEventMouseButton && @event.IsPressed()) {
			GetTree().ChangeSceneToPacked(inventory);
		}
	}
}