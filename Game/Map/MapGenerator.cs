using Godot;
using System;
using System.Linq;

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
        // RNG to determine if the path will be branched
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Seed = 10;

        // Get map node
        Map _Map = (Map)GetNode("/root/World/Map");

        // Get marker and level nodes for duplicating
        LevelMarker marker = (LevelMarker)GetNode("/root/World/Map/LevelMarker");
        marker.Disabled = true;
        Level level = (Level)GetNode("/root/World/Level");

        // Loop for the number of levels in the game
        for(int i = 0; i < NUM_LEVELS - 1; i++){
            // If path branches
            if(rng.RandiRange(1, 4) == 1 && i != 0){
                // Create new levels
                Level new_level1 = (Level)level.Duplicate();
                new_level1.Visible = true;
                GetNode("/root/World/Map").AddChild(new_level1);
                // TODO: CONSTRUCT ENEMY
                // new_level1.enemy = enemy1;
                // TODO: CONSTRUCT CARD REWARDS
                // new_level1.rewards = rewards1;
                Level new_level2 = (Level)level.Duplicate();
                new_level2.Visible = true;
                GetNode("/root/World/Map").AddChild(new_level2);
                // TODO: CONSTRUCT ENEMY
                // new_level2.enemy = enemy2;
                // TODO: CONSTRUCT CARD REWARDS
                // new_level2.rewards = rewards2;
                _Map.Levels = _Map.Levels.Append(new_level1).ToArray();
                _Map.Levels = _Map.Levels.Append(new_level2).ToArray();

                // Create new markers on the map
                LevelMarker new_marker1 = (LevelMarker)marker.Duplicate();
                new_marker1.Position = new Vector2(30 + 100 * i, 350);
                new_marker1.Level_number = i;
                new_marker1.Visible = true;
                GetNode("/root/World/Map").AddChild(new_marker1);
                LevelMarker new_marker2 = (LevelMarker)marker.Duplicate();
                new_marker2.Position = new Vector2(30 + 100 * i, 550);
                new_marker2.Level_number = i;
                new_marker2.Visible = true;
                GetNode("/root/World/Map").AddChild(new_marker2);
            } else {
                // Create new level
                Level new_level = (Level)level.Duplicate();
                new_level.Visible = true;
                GetNode("/root/World/Map").AddChild(new_level);
                _Map.Levels = _Map.Levels.Append(new_level).ToArray();
                // TODO: CONSTRUCT ENEMY
                // new_level.enemy = enemy;
                // TODO: CONSTRUCT CARD REWARDS
                // new_level.rewards = rewards;

                // Create new marker on the map
                LevelMarker new_marker = (LevelMarker)marker.Duplicate();
                new_marker.Position = new Vector2(30 + 100 * i, 450);
                new_marker.Level_number = i;
                new_marker.Visible = true;
                GetNode("/root/World/Map").AddChild(new_marker);
            }
        }
        // Create boss level
        {
            Level new_level = (Level)level.Duplicate();
            new_level.Visible = true;
            GetNode("/root/World/Map").AddChild(new_level);
            _Map.Levels = _Map.Levels.Append(new_level).ToArray();
            // TODO: CONSTRUCT ENEMY
            // new_level.enemy = enemy;
            // TODO: CONSTRUCT CARD REWARDS
            // new_level.rewards = rewards;

            // Create new marker on the map
            LevelMarker new_marker = (LevelMarker)marker.Duplicate();
            new_marker.Position = new Vector2(30 + 100 * (NUM_LEVELS - 1), 450);
            new_marker.Level_number = _Map.Levels.Length - 1;
            Image image = Image.LoadFromFile("res://Resources/Testing/temp_boss.png");
            ImageTexture texture = ImageTexture.CreateFromImage(image);
            Image image_hov = Image.LoadFromFile("res://Resources/Testing/temp_boss_hover.png");
            ImageTexture texture_hov = ImageTexture.CreateFromImage(image_hov);
            new_marker.TextureNormal = texture;
            new_marker.TextureHover = texture_hov;
            new_marker.Visible = true;
            GetNode("/root/World/Map").AddChild(new_marker);
        }
    }
}
