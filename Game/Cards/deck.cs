using Godot;
using System;

public partial class Deck : CardContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GD.Print("I do not understand");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_icon_input_event(Node viewport, InputEvent @event, int shape_idx){
        if (@event.IsActionPressed("click"))
        {
            ShowCards();
		}
    }

    public void _on_close_x_input_event(Node viewport, InputEvent @event, int shape_idx){
        if (@event.IsActionPressed("click"))
        {
            HideCards();
            GD.Print("asdfa");
		}
    }
}
