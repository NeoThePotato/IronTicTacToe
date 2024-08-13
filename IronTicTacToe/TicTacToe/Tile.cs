using DefaultRenderer.Defaults;
using IronEngine;

namespace TicTacToe
{
	public class Tile : RenderableTile, ICommandAble.IHasKey
	{
		public string? Key => $"{Position.x}{Position.y}";

		public string Description => $"Tile at ({ToString()}).";

		public override string ToString() => $"{Position.x}, {Position.y}";
	}
}
