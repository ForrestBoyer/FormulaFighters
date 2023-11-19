using Godot;
using System;

public partial class card : Node2D
{
	private bool draggable = false;
	private bool is_inside_dropable = false;
	private drag Drag;
	private Vector2 finalPos;
	private Vector2 offset;
	private Vector2 initialPos;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Drag = GetNode<drag>("/root/drag");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (draggable) {
			if (Input.IsActionJustPressed("click")) {
				initialPos = GlobalPosition;
				offset = GetGlobalMousePosition() - GlobalPosition;
				Drag.is_dragging = true;
			}

			if (Input.IsActionPressed("click")) {
				GlobalPosition = GetGlobalMousePosition() - offset;

			} else if (Input.IsActionJustReleased("click")) {
				Drag.is_dragging = false;
				var tween = GetTree().CreateTween();

				if (is_inside_dropable) {
					tween.TweenProperty(this, "position", finalPos, 0.2f).SetEase(Tween.EaseType.Out);
				}
				else {
					tween.TweenProperty(this, "position", initialPos, 0.2f).SetEase(Tween.EaseType.Out);
				}
			}
		}
	}

	public void _on_drag_detector_body_entered(Node2D body) {
		if (body.IsInGroup("dropable")) {
			is_inside_dropable = true;
			body.Modulate = new Color("DARK_ORANGE", 1);
			finalPos = body.Position;
		}
	}

	public void _on_drag_detector_body_exited(Node2D body) {
		if (body.IsInGroup("dropable")) {
			is_inside_dropable = false;
			body.Modulate = new Color("ORANGE_RED", 1);
		}
	}

	public void _on_drag_detector_mouse_entered() {
		if(!Drag.is_dragging) {
			draggable = true;
			Scale = new Vector2(4.3f, 4.3f);
		}
	}

	public void _on_drag_detector_mouse_exited() {
		if(!Drag.is_dragging) {
			draggable = false;
			Scale = new Vector2(4.0f, 4.0f);
		}
	}
}
