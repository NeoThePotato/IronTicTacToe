using DefaultRenderer.Defaults;
using IronEngine;

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
}
