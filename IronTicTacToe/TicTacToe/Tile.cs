using DefaultRenderer.Defaults;
using IronEngine;

namespace TicTacToe
{
	public class Tile : RenderableTile, ICommandAble.IHasKey
	{
		private static readonly char[] X_TO_CHAR = ['L', 'M', 'R'];
		private static readonly char[] Y_TO_CHAR = ['U', 'M', 'D'];

		public string? Key => ToString();

		public string Description => $"Tile at ({ToString()}).";

		public override string ToString() => $"{X_TO_CHAR[Position.x]}{Y_TO_CHAR[Position.y]}";
	}
}
