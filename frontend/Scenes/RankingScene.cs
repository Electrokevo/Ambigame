using Godot;
using Godot.Collections;
using System;
using System.Diagnostics.Metrics;
using System.Text; // For encoding/decoding response body
using System.Text.Json; // For parsing JSON responses (optional)


public partial class RankingScene : MarginContainer
{
	private static readonly string url = "http://localhost:3000/";
	[Export] private HttpRequest httpRequest;
	private TextEdit _username;
	private VBoxContainer _rankingContainer;
	private Dictionary _rankingData;
	public void Volver()
	{
		GetTree().ChangeSceneToFile("res://Scenes/MainScene.tscn");
	}

	public override void _Ready()
	{
		_rankingContainer = GetNode<VBoxContainer>("VBoxContainer").GetNode<VBoxContainer>("Ranking");

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
					_rankingData = (Dictionary)data["ranking"];
					CreateRankingRows(_rankingContainer);
				}
				catch (Exception e)
				{
					GD.PrintErr($"Error parsing JSON: {e.Message}");
					_rankingContainer.AddChild(errorBox());

				}
			}
		}
		else
		{
			GD.PrintErr($"HTTP Request failed with result: {result} and response code: {responseCode}");
			_rankingContainer.AddChild(errorBox());
		}
	}

	private void CreateRankingRows(VBoxContainer vBox)
	{

		for (int i = 0; i < 3; i++)
		{
			HBoxContainer dataRow = CreateRankingRow();
			renderData(dataRow, i + 1);
			vBox.AddChild(dataRow);
		}
	}

	private void renderData(HBoxContainer hbox, int position)
	{
		LabelSettings mySettings = GD.Load<LabelSettings>("res://RankingNumber.tres");
		CompressedTexture2D myTexture = GD.Load<CompressedTexture2D>("res://icon.png");

		foreach (var child in hbox.GetChildren())
		{
			if (child is Label label)
			{
				label.Text = $"#{position}";
				label.LabelSettings = mySettings;
			}
			else if (child is TextureRect img)
			{
				img.Texture = myTexture;
			}
			else if (child is VBoxContainer vbox)
			{
				RenderDataRows(vbox, position - 1);
			}
		}
	}

	private void RenderDataRows(VBoxContainer vBox, int playerCount)
	{
		LabelSettings mySettings = GD.Load<LabelSettings>("res://blackText.tres");

		Godot.Collections.Array playerData = (Godot.Collections.Array)_rankingData["global_ranking"];

		int counter = 0;

		foreach (HBoxContainer hBox in vBox.GetChildren())
		{
			foreach (Label label in hBox.GetChildren())
			{
				label.LabelSettings = mySettings;
				Dictionary player = (Dictionary)playerData[playerCount];

				if (counter == 0)
					label.Text = player["username"].ToString();
				else if (counter == 1)
					label.Text = player["total_score"].ToString();
				else if (counter == 2)
				{
					if (player["best_score_date"].ToString() != "")
					{
						String date = player["best_score_date"].ToString().Split('T')[0];
						label.Text = date.Replace("-", "/");
					}
					else
						label.Text = "N/A";
				}
				else if (counter == 3)
					label.Text = "Puntos";
				counter++;
			}
		}
	}

	private HBoxContainer CreateRankingRow()
	{
		HBoxContainer dataBox = new HBoxContainer();
		VBoxContainer rankingInfo = new VBoxContainer();

		dataBox.AddThemeConstantOverride("separation", 20);
		dataBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		dataBox.SizeFlagsVertical = SizeFlags.ExpandFill;

		rankingInfo.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		rankingInfo.Alignment = BoxContainer.AlignmentMode.Center;

		Label number = new Label();
		TextureRect img = new TextureRect();

		dataBox.AddChild(number);
		dataBox.AddChild(img);
		CreateRankingInfoRow(rankingInfo);
		dataBox.AddChild(rankingInfo);

		return dataBox;
	}

	private void CreateRankingInfoRow(VBoxContainer vBox)
	{
		for (int i = 0; i < 2; i++)
		{
			HBoxContainer dataRow = new HBoxContainer();

			/* if (i == 0)
				dataRow.Name = $"DataRow";
			else
				dataRow.Name = $"DataRow{i + 1}";*/
			Label rowInfo = new Label();
			Label rowInfo1 = new Label();

			rowInfo.SizeFlagsHorizontal = SizeFlags.ExpandFill;
			rowInfo1.SizeFlagsHorizontal = SizeFlags.ExpandFill;

			rowInfo1.HorizontalAlignment = HorizontalAlignment.Right;

			dataRow.AddChild(rowInfo);
			dataRow.AddChild(rowInfo1);

			vBox.AddChild(dataRow);
		}

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

		label.Text = "Error al cargar el ranking";
		vbox.AddChild(label);
		return vbox;
		
		
	}
}
