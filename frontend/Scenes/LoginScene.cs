using Godot;
using System;
using System.Text;
using Newtonsoft.Json;
using Snakes.Models;

public partial class LoginScene : MarginContainer
{
	[Export] private LineEdit UsernameBox;
	[Export] private LineEdit PasswordBox;
	[Export] private HttpRequest HTTPRequest;
	private static readonly string url = "http://localhost:3000/";

	override public void _Ready()
	{
		HTTPRequest.RequestCompleted += OnRequestCompleted;
	}
	
	public void GoToMain()
	{
		GetTree().ChangeSceneToFile("res://Scenes/MainScene.tscn");
	}

	public void GoToRegister()
	{
		GetTree().ChangeSceneToFile("res://Scenes/RegisterScene.tscn");
	}

	public void LogIn()
	{
		string[] headers = ["Content-Type: application/json"];
		string body = JsonConvert.SerializeObject(new
		{
			username = UsernameBox?.Text,
			password = PasswordBox?.Text
		});
		GD.Print(body);
		HTTPRequest.Request($"{url}players/login", headers, HttpClient.Method.Post, body) ;
	}
	
	private void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (responseCode == 200)
		{
			var json = new Json();
			json.Parse(body.GetStringFromUtf8());
			var response = json.GetData().AsGodotDictionary();
			Player.SetInstance(new Player
			{
				id =  response["player"].AsGodotDictionary()["id"].AsString(),
				username = response["player"].AsGodotDictionary()["username"].AsString(),
			});
			GD.Print($"Success, the request returned {responseCode}");
			GetTree().ChangeSceneToFile("res://Scenes/MainScene.tscn");
		}
		else if (responseCode == 401)
		{
			GD.Print("You are not logged in");
		}
		else if (responseCode == 404)
		{
			GD.Print("The server is down");
		}
		else if (responseCode == 0)
		{
			GD.Print("There was not a response (prende el backend!)");
		}
		else
		{
			GD.Print($"Error, the request returned {responseCode}");
		}
	}
}
