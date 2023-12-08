using System.Globalization;
using Godot;
using System;
using System.Reflection.Emit;

public partial class new_card : Node2D
{
	// Card int value
	public int intVal;

	// Card operator value
	public string opVal;

	public enum CardType
	{
		Number,
		Operator
	};

	public CardType cardType;

	// ID number for card
	private int cardID;

	// Card display text
	private Godot.Label displayText;
	// Initial card position when clicked
	private Vector2 initialPos;
	// Reset position for card when "reset equation" is pressed
	private Vector2 resetPos;
	// Global position of card slot
	private Vector2 slotPos;
	// Offset between card and mouse when dragging
	private Vector2 offset;
	// Is card being dragged
	private bool _dragging = false;
	// Is card inside card slot
	private bool is_inside_slot = false;

	// Helper methods for initializing cards after they are instantiated
	public void InitCard() 
	{
		intVal = -1;
		opVal = "NULL";
		cardID = -1;
	}

	public void InitCard(int IntVal, int ID) 
	{
		intVal = IntVal;
		cardType = CardType.Number;
		cardID = ID;
	}

	public void InitCard(string OpVal, int ID) 
	{
		opVal = OpVal;
		cardType = CardType.Operator;
		cardID = ID;
	}

	public int GetID() 
	{
		return cardID;
	}

	public void SetPos(Vector2 position) 
	{
		initialPos = position;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		displayText = (Godot.Label)FindChild("Label");
		if (cardType == CardType.Number) 
		{
			displayText.Text = intVal.ToString();
		} 
		else if (cardType == CardType.Operator) 
		{
			displayText.Text = opVal;
		}

		Position = initialPos;
		resetPos = Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// If card is dragging
		if (_dragging) {
			// Card follows mouse position
			Position = GetGlobalMousePosition() - offset;
		}
	}

	// When mouse is hovering over card
	public void _on_collider_mouse_entered() 
	{
		// Increase size of card
		Scale = new Vector2(1.1f, 1.1f);
	}

	// When mouse stops hovering over card
	public void _on_collider_mouse_exited() 
	{
		// Reset size of card
		Scale = new Vector2(1.0f, 1.0f);
	}

	// When mouse left clicks on card
	public void _on_collider_input_event(Node viewport, InputEvent @event, int shape_idx) 
	{
		// If left click is pressed
		if (@event.IsActionPressed("click")) {
			// Initial position current position
			initialPos = Position;
			// Offset to center card on mouse cursor
			offset = GetGlobalMousePosition() - GlobalPosition;
			// Card is dragging
			_dragging = true;
		}
		// If left click is released
		if (@event.IsActionReleased("click")) {
			// Card is no longer dragging
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
	public void _on_collider_body_entered(Node2D body) 
	{
		// If slot is available
		if (body.IsInGroup("Available")) 
		{
			// Card is inside slot
			is_inside_slot = true;
			// Slot position = Global position of card slot detected
			slotPos = body.Position;
			// Slot is now unavailable
			body.RemoveFromGroup("Available");
			body.AddToGroup("Unavailable");
		} 
		else 
		{
			// Card is not inside an available card slot
			is_inside_slot = false;
		}
	}

	// When card exits a card slot
	public void _on_collider_body_exited(Node2D body) 
	{
		// If slot is unavailable and card is inside another valid slot
		if (body.IsInGroup("Unavailable") && is_inside_slot == true) 
		{
			// Slot is now available
			body.RemoveFromGroup("Unavailable");
			body.AddToGroup("Available");
			// Card is not inside a slot
			is_inside_slot = false;
		}
	}
}
