using DefaultRenderer.Defaults;

namespace TicTacToe
{
	internal class Marker : RenderableTileObject
	{
		private static readonly string[] X_MARKER = [
				@"\  /",
				@" || ",
				@"/  \"
				];
		private static readonly string[] O_MARKER = [
				@"/--\",
				@"|  |",
				@"\__/"
				];
		private const int X_COLOR = 4;
		private const int O_COLOR = 5;
		public Marker(Player player) : base(player)
		{
			if (player.PlayerMarker == 'X')
			{
				Chars = X_MARKER;
				FgColor = X_COLOR;
			}
			else
			{
				Chars = O_MARKER;
				FgColor = O_COLOR;
			}
		}
	}
}
