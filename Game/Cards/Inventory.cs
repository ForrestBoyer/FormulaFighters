// IVENTORY
// Initializes with cards 1-9, two +'s, two -'s, and one x
// Displays an inventory icon that, when clicked, displays all of the cards in the inventory
// Cards displayed in a 12 x N grid
// X Button hides cards
// Inventory.InitCard(int or string) will create a new card and add it

using Godot;

public partial class Inventory : CardContainer
{
    // Initial generation parameters
    private int STARTING_INVENTORY_SIZE = 20;
	private int MAX_STARTING_NUM = 10;
	private int CHANCE_OF_STARTING_MULT_OP = 5;

    // Positioning Variables
    private int leftSideOffset = 100;
    private int spaceHorizontal = 100;
    private int spaceVertical = 75;
    private int rowSize = 12;
    // Create a Number Card for the Inventory
    private void InitCard(int val){
        // Create Card
        PackedScene cardScene = GD.Load<PackedScene>("res://Game/Cards/card.tscn");
        Card newCard = cardScene.Instantiate<Card>();
        // Initalize Card
        newCard.InitCard(val, size);
        newCard.MoveTo(new Vector2(size % rowSize * spaceHorizontal + leftSideOffset, 
                                   size / rowSize * spaceHorizontal + leftSideOffset));
        newCard._isDraggable = false;
        newCard.Visible = false;
        // Add it to Inventory
        AddCard(newCard);
        AddChild(newCard);
    }

    // Create an Operator Card for the Inventory
    private void InitCard(string val){
        // Create Card
        PackedScene cardScene = GD.Load<PackedScene>("res://Game/Cards/card.tscn");
        Card newCard = cardScene.Instantiate<Card>();
        // Initalize Card
        newCard.InitCard(val, size);
        newCard.MoveTo(new Vector2(size % rowSize * spaceHorizontal + leftSideOffset, 
                                   size / rowSize * spaceHorizontal + leftSideOffset));
        newCard._isDraggable = false;
        newCard.Visible = false;
        // Add it to Inventory
        AddCard(newCard);
        AddChild(newCard);
    }

    private void ShowCards()
    {
        GetNode<ColorRect>("ColorRect").Visible = false;
        GetNode<Label>("CloseX").Visible = true;
        foreach(Card c in Cards)
        {
            c.Visible = true;
        }
    }

    private void HideCards(){
        GetNode<ColorRect>("ColorRect").Visible = true;
        GetNode<Label>("CloseX").Visible = false;
        foreach(Card c in Cards)
        {
            c.Visible = false;
        }
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        // Create Starting Number Cards 1-9
        for(int i = 1; i <= 9; i++)
        {
            InitCard(i);
        }
        // Create Starting Operator Cards 2 +'s, 2 -'s, 1 x
        InitCard("+");
        InitCard("+");
        InitCard("-");
        InitCard("-");
        InitCard("x");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_color_rect_gui_input(InputEvent @event){
        if (@event.IsActionPressed("click"))
        {
            ShowCards();
		}
    }

    public void _on_close_x_input_event(Node viewport, InputEvent @event, int shape_idx){
        if (@event.IsActionPressed("click"))
        {
            HideCards();
		}
    }
}
