using IronEngine;

namespace TicTacToe
{
	internal class Player(string name) : Actor, ICommandAble
	{
		public string Name { get; private set; } = name;

		public char PlayerMarker => this == TicTacToeRuntime.Instance.X ? 'X' : 'O';

		public Actor Actor => this;

		public IEnumerable<ICommandAble.Command> GetAvailableActions()
		{
			var board = Runtime.Instance.TileMap;
			foreach (var tile in board.Where(t => !t.HasObject).Cast<Tile>())
				yield return new ICommandAble.Command(() => PlaceMarker(tile), $"Place '{PlayerMarker}' at {tile}.", tile.Key);
		}

		private void PlaceMarker(Tile tile)
		{
			tile.Object = new Marker(this);
		}

		public override string ToString() => Name;
	}
}
