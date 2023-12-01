using Godot;
using System;

public partial class new_card : Node2D
{
	// Initial card position when clicked
	private Vector2 initialPos;
	// Global position of card slot
	private Vector2 slotPos;
	// Offset between card and mouse when dragging
	private Vector2 offset;
	// Is card being dragged
	private bool _dragging = false;
	// Is card inside card slot
	private bool is_inside_slot = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		initialPos = Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// If card is dragging
		if (_dragging) {
			// Card position = mouse position
			Position = GetGlobalMousePosition() - offset;
		}
	}

	// When mouse is hovering over card
	public void _on_collider_mouse_entered() {
		// Increase size of card
		Scale = new Vector2(1.1f, 1.1f);
	}

	// When mouse stops hovering over card
	public void _on_collider_mouse_exited() {
		// Reset size of card
		Scale = new Vector2(1.0f, 1.0f);
	}

	// When mouse left clicks on card
	public void _on_collider_input_event(Node viewport, InputEvent @event, int shape_idx) {
		// If click is pressed
		if (@event.IsActionPressed("click")) {
			// Card is dragging
			_dragging = true;
			// Initial position current position
			initialPos = Position;
			// Offset to center card on mouse cursor
			offset = GetGlobalMousePosition() - GlobalPosition;
		}
		// If left click is released
		if (@event.IsActionReleased("click")) {
			// Card is not dragging
			_dragging = false;
			if (is_inside_slot) {
				// Move card into card slot
				Position = slotPos.Lerp(Position, 0.0f);
			} else {
				// Move card back to initial position before drag
				Position = initialPos.Lerp(Position, 0.0f);
			}
		}
	}

	// When card is dragged over a card slot
	public void _on_collider_body_entered(Node2D body) {
		// Card is inside slot
		is_inside_slot = true;
		// Slot position = Global position of card slot detected
		slotPos = body.Position;
	}
	// When card is dragged past a card slot
	public void _on_collider_body_exited(Node2D body) {
		// Card is no longer inside card slot
		is_inside_slot = false;
	}
}
