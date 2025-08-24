using Godot;
using Godot.Collections;
using System;
using System.Diagnostics.Metrics;
using System.Text; // For encoding/decoding response body
using System.Text.Json; // For parsing JSON responses (optional)
public partial class HistorialScene : MarginContainer
{
	private static readonly string url = "http://localhost:3000/";
	[Export] private HttpRequest httpRequest;
	private TextEdit _username;
	private HBoxContainer _historyContainer;
	private VBoxContainer _firstColumn;
	private VBoxContainer _secondColumn;
	private Dictionary _historyData;


	public void Volver()
	{
		GetTree().ChangeSceneToFile("res://Scenes/MainScene.tscn");
	}

	public override void _Ready()
	{
		_historyContainer = GetNode<HBoxContainer>("HBoxContainer").GetNode<VBoxContainer>("VBoxContainer").GetNode<HBoxContainer>("HBoxContainer");
		_firstColumn = _historyContainer.GetNode<VBoxContainer>("MatchesFirst");
		_secondColumn = _historyContainer.GetNode<VBoxContainer>("MatchesSecond");

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
					_historyData = (Dictionary)data["ranking"];
					GD.Print(_historyData["player_matches"]);

					Godot.Collections.Array matchData = (Godot.Collections.Array)_historyData["player_matches"];

					if (matchData.Count != 0)
						CreateHistoryRows();
					else
						_historyContainer.AddChild(EmptyHistoryBox());
				}
				catch (Exception e)
				{
					GD.PrintErr($"Error parsing JSON: {e.Message}");
					_historyContainer.AddChild(errorBox());

				}
			}
		}
		else
		{
			GD.PrintErr($"HTTP Request failed with result: {result} and response code: {responseCode}");
			_historyContainer.AddChild(errorBox());
		}
	}

	private void CreateHistoryRows()
	{
		for (int i = 0; i < 4 && i < _historyData.Count; i++)
		{
			VBoxContainer dataRow = CreateHistoryBox();

			RenderDataRows(dataRow, i);

			if (i % 2 == 0)
				_firstColumn.AddChild(dataRow);
			else
				_secondColumn.AddChild(dataRow);
		}
	}

	private void RenderDataRows(VBoxContainer vbox, int matchCount)
	{
		LabelSettings mySettings = GD.Load<LabelSettings>("res://blackText.tres");

		Godot.Collections.Array matchData = (Godot.Collections.Array)_historyData["player_matches"];

		Dictionary player = (Dictionary)matchData[matchCount];
		GD.Print(player);

		int counter = 0;

		foreach (HBoxContainer hbox in vbox.GetChildren())
		{
			foreach (Label label in hbox.GetChildren())
			{
				label.LabelSettings = mySettings;

				if (counter == 0)
					label.Text = "Fecha:";
				else if (counter == 1)
				{
					if (player["created_at"].ToString() != "")
					{
						String date = player["created_at"].ToString().Split('T')[0];
						label.Text = date.Replace("-", "/");
					}
					else
						label.Text = "N/A";
				}
				else if (counter == 2)
					label.Text = "Puntos:";
				else if (counter == 3)
					label.Text = player["score"].ToString();
				else if (counter == 4)
					label.Text = "Reciclados:";
				else if (counter == 5)
				{
					if (player["recolected"].ToString() != "")
					{
						label.Text = player["recolected"].ToString();
					}
					else
						label.Text = "N/A";
				}
				else if (counter == 6)
					label.Text = "Tiempo:";
				else if (counter == 7)
				{
					if (player["time"].ToString() != "")
					{
						label.Text = player["time"].ToString();
					}
					else
						label.Text = "N/A";
				}
				counter++;
			}
		}
	}

	private VBoxContainer CreateHistoryBox()
	{
		VBoxContainer vbox = new VBoxContainer();
		vbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		vbox.SizeFlagsVertical = SizeFlags.ExpandFill;

		for (int i = 0; i < 4; i++)
		{
			createDataRow(vbox);
		}

		return vbox;
	}

	private void createDataRow(VBoxContainer vBox)
	{
		HBoxContainer dataRow = new HBoxContainer();

		Label rowInfo = new Label();
		Label rowInfo1 = new Label();

		rowInfo.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		rowInfo1.SizeFlagsHorizontal = SizeFlags.ExpandFill;

		rowInfo1.HorizontalAlignment = HorizontalAlignment.Right;

		dataRow.AddChild(rowInfo);
		dataRow.AddChild(rowInfo1);

		vBox.AddChild(dataRow);
	}

	private VBoxContainer errorBox()
	{
		VBoxContainer vbox = new VBoxContainer();
		Label label = new Label();

		vbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		vbox.SizeFlagsVertical = SizeFlags.ExpandFill;

		label.LabelSettings = GD.Load<LabelSettings>("res://blackText.tres");
		label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		label.HorizontalAlignment = HorizontalAlignment.Center;
		label.SizeFlagsVertical = SizeFlags.ExpandFill;
		label.VerticalAlignment = VerticalAlignment.Center;

		label.Text = "Error al cargar el historial";
		vbox.AddChild(label);
		return vbox;
	}
	
	private VBoxContainer EmptyHistoryBox()
	{
		VBoxContainer vbox = new VBoxContainer();
		Label label = new Label();

		vbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		vbox.SizeFlagsVertical = SizeFlags.ExpandFill;

		label.LabelSettings = GD.Load<LabelSettings>("res://blackText.tres");
		label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		label.HorizontalAlignment = HorizontalAlignment.Center;
		label.SizeFlagsVertical = SizeFlags.ExpandFill;
		label.VerticalAlignment = VerticalAlignment.Center;

		label.Text = "No hay partidas jugadas";
		vbox.AddChild(label);
		return vbox;
	}
}
