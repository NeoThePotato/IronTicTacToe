using IronEngine;
using DefaultRenderer.Defaults;
using static TicTacToe.TicTacToeRuntime;

namespace TicTacToe
{
	/// <summary>
	/// Line is an alias for <see cref="Tile"/>s in a single line; either vertically, horizontally, or diagonally.
	/// </summary>
	using Line = IEnumerable<Tile>;

	/// <summary>
	/// Represents a single tile in a <see cref="TicTacToe"/> <see cref="TileMap"/>.
	/// </summary>
	public class Tile : RenderableTile, ICommandAble.IHasKey
	{
		private static readonly string[] X_TO_STRING = ["Left", "Middle", "Right"];
		private static readonly string[] Y_TO_STRING = ["Top", "Center", "Bottom"];

		public string? Key => ToString();

		public string Description => $"Tile at ({ToStringLong()}).";

		public override string ToString() => $"{1 + Position.x + (BOARD_SIZE * (BOARD_SIZE - Position.y - 1))}";

		public string ToStringLong() => $"{Y_TO_STRING[Position.y]}-{X_TO_STRING[Position.x]}";
	}

	/// <summary>
	/// Contains functions for getting series of <see cref="Line"/>s. Useful for checking for win conditions.
	/// </summary>
	public static class TileExtensions
	{
		public static IEnumerable<Line> GetAllPossibleLines(this TileMap tileMap)
		{
			for (var row = 0; row < tileMap.SizeY; row++)
				yield return tileMap.GetRow(row);
			for (var column = 0; column < tileMap.SizeX; column++)
				yield return tileMap.GetColumn(column);
			yield return tileMap.GetDiagonal(true);
			yield return tileMap.GetDiagonal(false);
		}

		/// <returns>The <paramref name="row"/>th row in <paramref name="tileMap"/>.</returns>
		public static Line GetRow(this TileMap tileMap, int row)
		{
			for (int y = 0; y < tileMap.SizeX; y++)
				yield return tileMap[new(row, y)] as Tile;
		}

		/// <returns>The <paramref name="column"/>th column in <paramref name="tileMap"/>.</returns>
		public static Line GetColumn(this TileMap tileMap, int column)
		{
			for (int x = 0; x < tileMap.SizeY; x++)
				yield return tileMap[new(x, column)] as Tile;
		}

		/// <returns>
		/// The down-right diagonal in <paramref name="tileMap"/> if <paramref name="downright"/> is <see langword="true"/>.
		/// Returns the down-left diagonal otherwise.
		/// </returns>
		public static Line GetDiagonal(this TileMap tileMap, bool downright)
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
