//By: James Singleton
//Date: 8/12/2023
using Godot;
using System;

public partial class HUD : CanvasLayer
{
	/// <summary>
	/// Used to refer to the ScoreLabel
	/// </summary>
	private Label _scoreLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//stores a refrence of the ScoreLabel in _scoreLabel
		_scoreLabel = GetNode<Label>("ScoreLabel");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	/// <summary>
	/// Increases the score by n
	/// </summary>
	/// <param name="n">The value the Score is increased by</param>
	public void IncrementScore(int n)
	{
		/*converts the current string in the Text field of ScoreLabel to int
		  then adds n. Then finally converts the sum into a string and stores it
		  back into the the Text field of ScoreLabel*/
		_scoreLabel.Text = (Int32.Parse(_scoreLabel.Text) + n).ToString();
	}

	/// <summary>
	/// Decreases the score by n
	/// </summary>
	/// <param name="n">The value the Score is decreased by</param>
	public void DecrementScore(int n)
	{
		/*converts the current string in the Text field of ScoreLabel to int
		  then subtracts n. Then finally converts the difference into a string
		  and stores it back into the the Text field of ScoreLabel*/
		_scoreLabel.Text = (Int32.Parse(_scoreLabel.Text) - n).ToString();
	}

	/// <summary>
	/// Resets the score back to Zero
	/// </summary>
	public void ResetScore()
	{
		_scoreLabel.Text = "0";
	}

	/// <summary>
	/// Changes the MessageLabels Text field to message
	/// </summary>
	/// <param name="message">What Message Label will display</param>
	public void UpdateMessage(string message)
	{
		Label messageLabel = GetNode<Label>("MessageLabel");
		messageLabel.Text = message;
	}
}
