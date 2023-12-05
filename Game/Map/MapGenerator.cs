using Godot;
using System;

public partial class MapGenerator : Node
{
    // The chance of levels branching (1/BRANCH_CHANCE)
    const int BRANCH_CHANCE = 4;
    // The chance of a reward being an operator (1/OPERATOR_CHANCE)
    const int OPERATOR_CHANCE = 3;
    // The number of levels in the game
    const int NUM_LEVELS = 10;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{   
        Generate();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    // Generates a new map
    public void Generate(){

        int leftSideOffset = 50;
        int spaceHorizontal = 125;
        int spaceVertical = 50;
        int centerScreen = 360;

        // RNG to determine if the path will be branched
        Random rng = new Random();

        // Get map node
        Map _Map = GetNode<Map>("/root/World/Map");

        // Get marker and level nodes for duplicating
        LevelMarker marker = GetNode<LevelMarker>("/root/World/Map/LevelMarker");
        marker.Disabled = true;

        // Loop for the number of levels in the game
        for(int i = 0; i < NUM_LEVELS - 1; i++){
            // If path branches
            if(rng.Next(1, 4) == 1 && i != 0){
                // ------------- Create new markers on the map ----------------
                // Bottom Marker
                LevelMarker new_marker1 = (LevelMarker)marker.Duplicate();
                new_marker1.Position = new Vector2(leftSideOffset + spaceHorizontal * i, centerScreen - spaceVertical);
                new_marker1.Level_number = i;
                new_marker1.Visible = true;
                GetNode("/root/World/Map").AddChild(new_marker1);

                // Top Marker
                LevelMarker new_marker2 = (LevelMarker)marker.Duplicate();
                new_marker2.Position = new Vector2(leftSideOffset + spaceHorizontal * i, centerScreen + spaceVertical);
                new_marker2.Level_number = i;
                new_marker2.Visible = true;
                GetNode("/root/World/Map").AddChild(new_marker2);
            }
            // If path doesn't branch
            else
            {
                // Create new marker on the map
                LevelMarker new_marker = (LevelMarker)marker.Duplicate();
                new_marker.Position = new Vector2(leftSideOffset + spaceHorizontal * i, centerScreen);
                new_marker.Level_number = i;
                new_marker.Visible = true;
                GetNode("/root/World/Map").AddChild(new_marker);
            }
        }
        // Create boss level
        {
            // Create new marker on the map
            LevelMarker new_marker = (LevelMarker)marker.Duplicate();
            new_marker.Position = new Vector2(leftSideOffset + spaceHorizontal * (NUM_LEVELS - 1), centerScreen);
            new_marker.Level_number = _Map.Levels.Count - 1;

            // Boss texture stuff
            Image image = Image.LoadFromFile("res://Resources/Testing/temp_boss.png");
            Image image_hov = Image.LoadFromFile("res://Resources/Testing/temp_boss_hover.png");
            ImageTexture texture = ImageTexture.CreateFromImage(image);
            ImageTexture texture_hov = ImageTexture.CreateFromImage(image_hov);
            new_marker.TextureNormal = texture;
            new_marker.TextureHover = texture_hov;

            new_marker.Visible = true;
            GetNode("/root/World/Map").AddChild(new_marker);
        }
    }
}
