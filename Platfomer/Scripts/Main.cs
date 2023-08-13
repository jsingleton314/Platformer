//By: James Singleton
//Date: 8/12/2023
using Godot;
using System;

public partial class Main : Node
{
	/// <summary>
	/// Allows the player to swapped out with a different scene
	/// </summary>
	[Export]
	public PackedScene PlayerScene {get; set;}

	private Node2D _currentLevel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//When player picks a level the game knows which level to load
		((MainMenu)GetNode<MainMenu>("MainMenu")).Connect(MainMenu.SignalName.StartLevel, new Callable(this, nameof(this.OnMainMenuStartLevelEventHandler)));

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	/// <summary>
	/// Loads a level into the scene and sets up the UI for the level as well as adding the player to the scene
	/// </summary>
	/// <param name="levelNum">The levels number in the scenes title "Level_x" where x is levelNum</param>
	private void LoadLevel (int levelNum)
	{
		//hides main menu
		GetNode<MainMenu>("MainMenu").Hide();
		//put the hud into an refrence variable to manipulate it
		HUD hud = (HUD) GetNode<CanvasLayer>("Hud");

		hud.ResetScore();//sets the score label to 0

		//displays the controls to the player
		hud.UpdateMessage("Move Left: Left Arrow Key     Move Right: Right Arrow Key\nShoot Left: A     Shoot Right: D\nJump: Space Bar");

		hud.Show();

		//loads the level from the Levels folder and adds it to the scene
		PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/Levels/Level_" + levelNum + ".tscn");
		_currentLevel = (Node2D) scene.Instantiate();//allows the current level to be tracked through several functions
		AddChild(_currentLevel );

		//gets all the mobs in the scene and makes sure that their death signal will be picked up by Main
		Godot.Collections.Array<Node> mobList = getMobs();
		foreach(Node node in mobList)
		{
			Mob mob = (Mob) node;
			mob.Connect(Mob.SignalName.MobSlain, new Callable(this, nameof(this.MobSlainEventHandler)));
		}

		//gets the marker node for the level tha represents the player start then adds the player to the scene at their spawnpoint
		Marker2D playerSpawn = _currentLevel.GetNode<Marker2D>("PlayerStart");
		Player player = PlayerScene.Instantiate<Player>();
		player.Position = playerSpawn.Position;
		AddChild(player);

		//Ensures Main will hear main will be informed when the player dies or reaches the end of the level an react appropriately
		player.Connect(Player.SignalName.PlayerSlain, new Callable(this, nameof(this.PlayerSlainEventHandler)));
		player.Connect(Player.SignalName.PlayerReachedGoal, new Callable(this, nameof(this.PlayerReachedGoalEventHandler)));

		GetNode<Timer>("GameStartTimer").Start();//starts the time for how long the controls text stays on the screen
	}

	/// <summary>
	/// Unloads the current level and redisplays the main menu
	/// </summary>
	private void UnloadLevel()
	{
		_currentLevel.QueueFree();//removes the level from the scene and all its children
		_currentLevel = null;//ensures that _currentLevel refrences nothing

		//If the player is already removed from the scene then GetNode will throw an error 
		try
		{
			//removes the player from the scene since the level is over
			((Player) GetNode<CharacterBody2D>("Player")).QueueFree();
		}
		catch (Exception e) 
		{}

		GetNode<MainMenu>("MainMenu").Show();//returns player to the main menu
		

	}

	/// <summary>
	/// Get all the Mobs in the scene and put them into a Godot Array
	/// </summary>
	/// <returns>A Godot Array of Mobs</returns>
	private Godot.Collections.Array<Node> getMobs()
	{
		Godot.Collections.Array<Node>  mobList = null;
		if(_currentLevel != null)
		{
			mobList =  GetTree().GetNodesInGroup("Mob");
		}
		
		return mobList;
	}

	/// <summary>
	/// Loads the level the user selected from the main menu
	/// </summary>
	/// <param name="levelNum">The levels number in the scenes title "Level_x" where x is levelNum</param>
	private void OnMainMenuStartLevelEventHandler(int levelNum)
	{
		LoadLevel(levelNum);
	}

	/// <summary>
	/// When the GameOverTimer finishes counting down return the player to the main menu and unpause the game
	/// </summary>
	private void OnPlayerDiedTimerTimeout()
	{
		//unloads level and returns player to main menu
		UnloadLevel();

		GetTree().Paused = false;//unpauses the game

		GetNode<CanvasLayer>("Hud").Hide();//HUD not shown on main menu

	}

	/// <summary>
	/// Gets rid of the controls text after GameStartTimer finishes counting down
	/// </summary>
	private void OnGameStartTimerTimeout()
	{
		((HUD)GetNode<CanvasLayer>("Hud")).UpdateMessage("");
	}

	/// <summary>
	/// When the player scene dies let the player know they died and prepare to return to the main menu
	/// </summary>
	private void PlayerSlainEventHandler()
	{
		//gets the HUD node 
		HUD hud = (HUD) GetNode<CanvasLayer>("Hud");

		hud.UpdateMessage("You Died.");//tells the player they lost

		GetNode<Timer>("GameOverTimer").Start();//time used to determine how long the game will stay paused while the "You Died." message is shown

		GetTree().Paused = true;//pauses the game
		

	}

	/// <summary>
	/// When the player wins (by reaching the goal) Let them know and start the countdown to return to main menu
	/// </summary>
	private void PlayerReachedGoalEventHandler()
	{
		//Gets the HUD and tells it to tell the player they won.
		HUD hud = (HUD) GetNode<CanvasLayer>("Hud");
		hud.UpdateMessage("You Win!");

		GetNode<Timer>("GameOverTimer").Start();

		GetTree().Paused = true;
	}

	/// <summary>
	/// When a mob dies update the score
	/// </summary>
	/// <param name="mob">The mob that died</param>
	private void MobSlainEventHandler(Mob mob)
	{
		//Gets the HUD and tells it to update the player's score.
		HUD hud = (HUD) GetNode<CanvasLayer>("Hud");
		hud.IncrementScore(mob.ScoreValue);

	}

}
