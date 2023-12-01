using Godot;
using System;

public partial class reward_menu : Node2D
{
	private PackedScene levelSelect;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		levelSelect = (PackedScene)ResourceLoader.Load("res://Resources/Menus/level_select_menu.tscn");
	}

	public void _on_card_collider_input_event(Viewport view, InputEvent @event, int shape_idx) {
		if (@event is InputEventMouseButton && @event.IsPressed()) {
            GetTree().ChangeSceneToPacked(levelSelect);
        }
	}
}
