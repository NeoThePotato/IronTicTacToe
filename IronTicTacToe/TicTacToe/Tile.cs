using DefaultRenderer.Defaults;
using IronEngine;
using System.Data.Common;

namespace TicTacToe
{
	public class Tile : RenderableTile, ICommandAble.IHasKey
	{
		private static readonly char[] X_TO_CHAR = ['L', 'M', 'R'];
		private static readonly char[] Y_TO_CHAR = ['U', 'M', 'D'];
		private static readonly string[] X_TO_STRING = ["Left", "Middle", "Right"];
		private static readonly string[] Y_TO_STRING = ["Up", "Center", "Down"];

		public string? Key => ToString();

		public string Description => $"Tile at ({ToStringLong()}).";

		public override string ToString() => $"{X_TO_CHAR[Position.x]}{Y_TO_CHAR[Position.y]}";

		public string ToStringLong() => $"{X_TO_STRING[Position.x]}-{Y_TO_STRING[Position.y]}";
	}

	public static class TileExtensions
	{
		public static IEnumerable<IEnumerable<Tile>> GetAllPossibleChains(this TileMap tileMap)
		{
			for (var row = 0; row < tileMap.SizeY; row++)
				yield return tileMap.GetRow(row);
			for (var column = 0; column < tileMap.SizeX; column++)
				yield return tileMap.GetColumn(column);
			yield return tileMap.GetDiagonal(true);
			yield return tileMap.GetDiagonal(false);
		}

		public static IEnumerable<Tile> GetRow(this TileMap tileMap, int row)
		{
			for (int y = 0; y < tileMap.SizeX; y++)
				yield return tileMap[new(row, y)] as Tile;
		}

		public static IEnumerable<Tile> GetColumn(this TileMap tileMap, int column)
		{
			for (int x = 0; x < tileMap.SizeY; x++)
				yield return tileMap[new(x, column)] as Tile;
		}

		public static IEnumerable<Tile> GetDiagonal(this TileMap tileMap, bool downright)
		{
			if (downright)
			{
				for (int x = 0; x < tileMap.SizeX; x++)
					yield return tileMap[new(x, x)] as Tile;
			}
			else
			{
				for (int x = 0; x < tileMap.SizeX; x++)
					yield return tileMap[new(x, tileMap.SizeY - x - 1)] as Tile;
			}
		}
	}
}
