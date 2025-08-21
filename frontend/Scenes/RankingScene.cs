using Godot;
using Godot.Collections;
using System;
	using System.Text; // For encoding/decoding response body
	using System.Text.Json; // For parsing JSON responses (optional)


public partial class RankingScene : MarginContainer
{
	private static readonly string url = "http://localhost:3000/";
	HttpRequest httpRequest;
	private TextEdit _username;
	public void Volver()
	{
		GetTree().ChangeSceneToFile("res://Scenes/MainScene.tscn");
	}

	public override void _Ready()
	{
		httpRequest = GetNode<HttpRequest>("HTTPRequest");
		httpRequest.RequestCompleted += HttpRequestCompleted;

		httpRequest.Request($"{url}players/1/ranking");
	}
	
	private void HttpRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
		{
			if (result == (long)Error.Ok)
			{
				string responseBody = Encoding.UTF8.GetString(body);
				GD.Print($"Request completed with code: {responseCode}");

				// Example of parsing JSON response (if applicable)
				if (responseCode == 200 && responseBody.StartsWith("{") && responseBody.EndsWith("}"))
				{
					try
					{
						var json = new Json();
						json.Parse(responseBody);
						var data = json.GetData().AsGodotDictionary(); // Or deserialize to a custom C# class
						var dataR = (Dictionary) data["ranking"];
						GD.Print($"Parsed JSON data: {dataR["global_ranking"]}");
					}
					catch (Exception e)
					{
						GD.PrintErr($"Error parsing JSON: {e.Message}");
					}
				}
			}
			else
			{
				GD.PrintErr($"HTTP Request failed with result: {result} and response code: {responseCode}");
			}
		}
}
