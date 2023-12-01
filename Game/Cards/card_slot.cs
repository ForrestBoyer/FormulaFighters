using Godot;
using System;

public partial class card_slot : StaticBody2D
{
	// private StaticBody2D cardSlot;
	// private CollisionShape2D cardCollider;
	private ColorRect slotColor;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// cardSlot = GetNode<StaticBody2D>("/root/testLevel/CardSlot");
		// cardCollider = GetNode<CollisionShape2D>("CollisionShape2D");
		slotColor = GetNode<ColorRect>("ColorRect");
		slotColor.Color = new Color("ORANGE");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// When card is inside card slot
	public void _on_card_detector_area_entered(Area2D area) {
		// cardSlot.Visible = false;
		slotColor.Color = new Color("RED");
		// cardCollider.Disabled = true;
	}
	// When card has left card slot
	public void _on_card_detector_area_exited(Area2D area) {
		// cardSlot.Visible = true;
		slotColor.Color = new Color("ORANGE");
		// cardCollider.Disabled = false;
	}
}
