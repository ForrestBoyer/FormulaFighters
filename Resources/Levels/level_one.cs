using Godot;
using System;

public partial class level_one : Node2D
{
	private int ATTACK = 1;
	private PackedScene deckMenu;
	private PackedScene winMenu;
	private PackedScene gameoverMenu;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		deckMenu = (PackedScene)ResourceLoader.Load("res://Resources/Menus/deck_menu.tscn");
		winMenu = (PackedScene)ResourceLoader.Load("res://Resources/Menus/win_menu.tscn");
		gameoverMenu = (PackedScene)ResourceLoader.Load("res://Resources/Menus/game_over_menu.tscn");
	}

	public void _on_deck_collider_input_event(Viewport view, InputEvent @event, int shape_idx) {
		if (@event is InputEventMouseButton && @event.IsPressed()) {
			GetTree().ChangeSceneToPacked(deckMenu);
		}
	}

	public void _on_discard_collider_input_event(Viewport view, InputEvent @event, int shape_idx) {
		if (@event is InputEventMouseButton && @event.IsPressed()) {
			GetTree().ChangeSceneToPacked(deckMenu);
		}
	}

	public void _on_attack_defend_collider_input_event(Viewport view, InputEvent @event, int shape_idx) {
		if (@event is InputEventMouseButton && @event.IsPressed()) {
			if (ATTACK == 1) {
				GetNode<ColorRect>("AttackDefend").Color = new Color(255, 0, 0);
				GetNode<Label>("AttackDefend/Label6").Text = "Defend";
				ATTACK = 0;
			}
			else {
				GetNode<ColorRect>("AttackDefend").Color = new Color(0, 255, 0);
				GetNode<Label>("AttackDefend/Label6").Text = "Attack";
				ATTACK = 1;
			}
		}
	}

	public void _on_win_combat_pressed() {
		GetTree().ChangeSceneToPacked(winMenu);
	}

	public void _on_lose_combat_pressed() {
		GetTree().ChangeSceneToPacked(gameoverMenu);
	}
}
