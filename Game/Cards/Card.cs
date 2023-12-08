using System.Globalization;
using System.Reflection.Metadata;
using Godot;

public enum CardType
{
	Number,
	Operator
};

public partial class Card : Node2D
{
	// Card Information
	public int IntVal { get; set; }
	public string OpVal { get; set; }
	public CardType CardType { get; set; }
	private int cardID;
	private Label DisplayLabel { get; set; }

	// Card Position Stuff
	public Vector2 HomePosition { get; set; }
	private bool _isDragging = false;

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

	public void MoveTo(Vector2 position)
	{
		Position = position;
	}

	public bool IsOverCardSlot()
	{
		Level level = (Level)GetParent().GetParent();

		foreach (Node child in level.GetChildren())
		{
			if (child is CardSlot cardSlot)
			{
				if (cardSlot._isHovered)
				{
					return true;
				}
				else
				{
					return false;
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
		if (_isDragging)
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
			if (IsOverCardSlot())
			{
				GD.Print("Over slot");
			}
			else
			{
				MoveTo(HomePosition);
			}
		}
	}
}
