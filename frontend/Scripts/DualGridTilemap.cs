using Godot;
using System;
using System.Collections.Generic;

namespace Snake;

using static TileType;

public partial class DualGridTilemap : Node2D {
	[Export] TileMapLayer worldMapLayer;
	[Export] TileMapLayer displayMapLayer;
	[Export] public Vector2I grassPlaceholderAtlasCoord;
	[Export] public Vector2I dirtPlaceholderAtlasCoord;
	readonly Vector2I[] NEIGHBOURS = [new(0, 0), new(1, 0), new(0, 1), new(1, 1)];

	readonly Dictionary<Tuple<TileType, TileType, TileType, TileType>, Vector2I> neighboursToAtlasCoord = new() {
		{new (TrashBag, TrashBag, TrashBag, TrashBag), new Vector2I(2, 1)}, // All corners
		{new (Dirt, Dirt, Dirt, TrashBag), new Vector2I(1, 3)}, // Outer bottom-right corner
		{new (Dirt, Dirt, TrashBag, Dirt), new Vector2I(0, 0)}, // Outer bottom-left corner
		{new (Dirt, TrashBag, Dirt, Dirt), new Vector2I(0, 2)}, // Outer top-right corner
		{new (TrashBag, Dirt, Dirt, Dirt), new Vector2I(3, 3)}, // Outer top-left corner
		{new (Dirt, TrashBag, Dirt, TrashBag), new Vector2I(1, 0)}, // Right edge
		{new (TrashBag, Dirt, TrashBag, Dirt), new Vector2I(3, 2)}, // Left edge
		{new (Dirt, Dirt, TrashBag, TrashBag), new Vector2I(3, 0)}, // Bottom edge
		{new (TrashBag, TrashBag, Dirt, Dirt), new Vector2I(1, 2)}, // Top edge
		{new (Dirt, TrashBag, TrashBag, TrashBag), new Vector2I(1, 1)}, // Inner bottom-right corner
		{new (TrashBag, Dirt, TrashBag, TrashBag), new Vector2I(2, 0)}, // Inner bottom-left corner
		{new (TrashBag, TrashBag, Dirt, TrashBag), new Vector2I(2, 2)}, // Inner top-right corner
		{new (TrashBag, TrashBag, TrashBag, Dirt), new Vector2I(3, 1)}, // Inner top-left corner
		{new (Dirt, TrashBag, TrashBag, Dirt), new Vector2I(2, 3)}, // Bottom-left top-right corners
		{new (TrashBag, Dirt, Dirt, TrashBag), new Vector2I(0, 1)}, // Top-left down-right corners
		{new (Dirt, Dirt, Dirt, Dirt), new Vector2I(0, 3)}, // No corners
	};

	public override void _Ready() {
		// Refresh all display tiles
		foreach (Vector2I coord in worldMapLayer.GetUsedCells()) {
			SetDisplayTile(coord);
		}
	}

	/// <summary>
	/// <para>Returns the map coordinates of the cell containing the given <paramref name="localPosition"/>. If <paramref name="localPosition"/> is in global coordinates, consider using <see cref="Godot.Node2D.ToLocal(Vector2)"/> before passing it to this method. See also <see cref="Godot.TileMapLayer.MapToLocal(Vector2I)"/>.</para>
	/// </summary>
	public Vector2I LocalToMap(Vector2 pos)
	{
		return worldMapLayer.LocalToMap(pos);
	}

	public void SetTile(Vector2I coords, Vector2I atlasCoords) {
		worldMapLayer.SetCell(coords, 0, atlasCoords);
		SetDisplayTile(coords);
	}

	void SetDisplayTile(Vector2I pos) {
		// loop through 4 display neighbours
		for (int i = 0; i < NEIGHBOURS.Length; i++) {
			Vector2I newPos = pos + NEIGHBOURS[i];
			displayMapLayer.SetCell(newPos, 1, CalculateDisplayTile(newPos));
		}
	}

	Vector2I CalculateDisplayTile(Vector2I coords) {
		// get 4 world tile neighbours
		TileType botRight = GetWorldTile(coords - NEIGHBOURS[0]);
		TileType botLeft = GetWorldTile(coords - NEIGHBOURS[1]);
		TileType topRight = GetWorldTile(coords - NEIGHBOURS[2]);
		TileType topLeft = GetWorldTile(coords - NEIGHBOURS[3]);

		// return tile (atlas coord) that fits the neighbour rules
		return neighboursToAtlasCoord[new(topLeft, topRight, botLeft, botRight)];
	}

	TileType GetWorldTile(Vector2I coords) {
		Vector2I atlasCoord = worldMapLayer.GetCellAtlasCoords(coords);
		if (atlasCoord == grassPlaceholderAtlasCoord)
			return TrashBag;
		else
			return Dirt;
	}
}

public enum TileType {
	None,
	TrashBag,
	Dirt,
	Terrain,
}
