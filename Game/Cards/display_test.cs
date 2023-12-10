using Godot;
using System;

public partial class display_test : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        DiscardPile discard = GetNode<DiscardPile>("DiscardPile");
        Deck deck = GetNode<Deck>("Deck");
        Inventory inventory = GetNode<Inventory>("Inventory");
        discard.SetCards(inventory.Cards);
        deck.SetCards(inventory.Cards);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
