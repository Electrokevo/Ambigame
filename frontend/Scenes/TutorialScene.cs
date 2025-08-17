using Godot;
using System;

public partial class TutorialScene : MarginContainer
{
	public void Volver()
	{
		GetTree().ChangeSceneToFile("res://Scenes/MainScene.tscn");
	} 
}
