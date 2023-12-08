using Godot;
using System;

public partial class CardSlot : Node2D
{ 
	public Card CardInSlot { get; set; }

	public bool SlotOpen { get; set; } = true;

	public bool _isHovered { get; set; } = false;

	private ColorRect slotColor;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		slotColor = GetNode<ColorRect>("ColorRect");
		slotColor.Color = new Color("ORANGE");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_card_detector_mouse_entered()
	{
		_isHovered = true;
	}

	public void _on_card_detector_mouse_exited()
	{
		_isHovered = false;
	}

}
