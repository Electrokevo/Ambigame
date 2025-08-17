using Godot;
using System;

public partial class HistorialScene : MarginContainer
{
	public void Volver()
	{
		GetTree().ChangeSceneToFile("res://Scenes/MainScene.tscn");
	}    
}
