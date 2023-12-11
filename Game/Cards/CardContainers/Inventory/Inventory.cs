// IVENTORY
// Initializes with cards 1-9, two +'s, two -'s, and one x
// Displays an inventory icon that, when clicked, displays all of the cards in the inventory
// Cards displayed in a 12 x N grid
// X Button hides cards
// Inventory.InitCard(int or string) will create a new card and add it

using Godot;
using System;

public partial class Inventory : CardContainer
{
    // Initial generation parameters
    private int STARTING_INVENTORY_SIZE = 20;
	private int MAX_STARTING_NUM = 10;
	private int CHANCE_OF_STARTING_MULT_OP = 5;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        base._Ready();

        PackedScene cardScene = GD.Load<PackedScene>("res://Game//Cards/card.tscn");
        Random rng = new Random();

        for (int i = 0; i < STARTING_INVENTORY_SIZE; i++)
		{
			Card card = cardScene.Instantiate<Card>();

			// Generate Number
			if (rng.Next(2) == 0)
			{
				card.InitCard(rng.Next(1, MAX_STARTING_NUM));
				AddCard(card);
			}
			// Generate Operator
			else
			{
				// 1 in CHANCE_OF_STARTING_MULT_OP chance of each operator being a * instead of a +
				if (rng.Next(CHANCE_OF_STARTING_MULT_OP) == 0)
				{
					card.InitCard("x");
				}
				else
				{
					card.InitCard("+");
				}
				AddCard(card);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_icon_input_event(InputEvent @event)
	{
        if (@event.IsActionPressed("click"))
        {
            ShowCards();
		}
    }

    public void _on_close_x_input_event(Node viewport, InputEvent @event, int shape_idx)
	{
        if (@event.IsActionPressed("click"))
        {
            HideCards();
		}
    }
}
