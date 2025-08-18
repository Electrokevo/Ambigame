using System;
using Godot;

public partial class RankingScene : MarginContainer
{
    public void Volver()
    {
        GetTree().ChangeSceneToFile("res://Scenes/MainScene.tscn");
    }
}
