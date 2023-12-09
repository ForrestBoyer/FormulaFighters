using System.Globalization;
using System.Reflection.Metadata;
using System;
using Godot;

public enum CardType
{
	Number,
	Operator
};

public partial class Card : Node2D
{
    // Probabilities for Random Card Generation (1/CHANCE)
    const int OPERATOR_CHANCE = 4;
    const int MULT_CHANCE = 4;
    // Maximum Value a Number Card can Hold
    const int MAX_NUM = 9;
    
	// Card Information
	public int IntVal { get; set; }
	public string OpVal { get; set; }
	public CardType CardType { get; set; }
	private int cardID;
	private Label DisplayLabel { get; set; }

	// Card Position Stuff
	public Vector2 HomePosition { get; set; }
    public bool _isDraggable = true;
	private bool _isDragging = false;
	public bool _isInCardSlot = false;
	private CardSlot CardSlot;

	public void InitCard() 
	{
		IntVal = -1;
		cardID = -1;
	}

	public void InitCard(int val, int id) 
	{
		IntVal = val;
		CardType = CardType.Number;
		cardID = id;
	}

	public void InitCard(string val, int id) 
	{
		OpVal = val;
		CardType = CardType.Operator;
		cardID = id;
	}

    public void InitCardRandom() 
	{
        // RNG for generating a random card
        Random rng = new Random();

        // If Generating Operator
        if(rng.Next(1, OPERATOR_CHANCE) == 1) {
		    CardType = CardType.Operator;
            if(rng.Next(1, MULT_CHANCE) == 1){
                // TODO: Edit to match current implementation
                OpVal = "x";
            } else if(rng.Next(1, 2) == 1){
                // TODO: Edit to match current implementation
                OpVal = "+";
            } else {
                // TODO: Edit to match current implementation
                OpVal = "-";
            }
        // If Generating Number
        } else {
            CardType = CardType.Number;
            IntVal = rng.Next(1, MAX_NUM);
        }
	}

	public void MoveTo(Vector2 position)
	{
		Position = position;
	}

	public bool IsOverCardSlot(Card card)
	{
		Level level = (Level)GetParent().GetParent();

		foreach (Node child in level.GetChildren())
		{
			if (child is CardSlot cardSlot)
			{
				if (cardSlot._isHovered && cardSlot.SlotOpen)
				{
					if (card.CardSlot != null)
					{
						card.CardSlot.SlotOpen = true;
						card.CardSlot = null;
					}
					cardSlot.SlotOpen = false;
					cardSlot.CardInSlot = card;
					card.Position = cardSlot.Position;
					card.CardSlot = cardSlot;
					return true;
				}
			}
		}

		return false;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DisplayLabel = (Godot.Label)FindChild("Label");
		if (CardType == CardType.Number) 
		{
			DisplayLabel.Text = IntVal.ToString();
		} 
		else if (CardType == CardType.Operator) 
		{
			DisplayLabel.Text = OpVal;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_isDragging && _isDraggable)
		{
			Position = GetGlobalMousePosition();
		}
	}

	public void _on_collider_mouse_entered()
	{
		Scale = new Vector2(1.1f, 1.1f);
	}

	public void _on_collider_mouse_exited()
	{
		Scale = new Vector2(1, 1);
	}

	public void _on_collider_input_event(Node viewport, InputEvent @event, int shape_idx)
	{
		if (@event.IsActionPressed("click"))
		{
			_isDragging = true;
		}

		if (@event.IsActionReleased("click"))
		{
			_isDragging = false;
			if (IsOverCardSlot(this))
			{
				_isInCardSlot = true;
			}
			else
			{
				MoveTo(HomePosition);
				if (CardSlot != null)
				{
					CardSlot.SlotOpen = true;
					CardSlot = null;
				}
			}
		}
	}
}
