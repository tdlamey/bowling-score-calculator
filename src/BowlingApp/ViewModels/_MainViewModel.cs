namespace BowlingApp.ViewModels
{
	/// <summary>
	/// A view model for binding to the main view.
	/// </summary>
	public class MainViewModel : ViewModelBase
	{
		/// <summary>
		/// Creates a new instance of <see cref="MainViewModel"/>.
		/// </summary>
		public MainViewModel()
		{
			ScoreEntryViewModel = new ScoreEntryViewModel();
			GameViewModel = new GameViewModel(ScoreEntryViewModel);
		}

		/// <summary>
		/// A view model for binding to the score input view.
		/// </summary>
		public ScoreEntryViewModel ScoreEntryViewModel { get; }

		/// <summary>
		/// A view model for binding to the game view.
		/// </summary>
		public GameViewModel GameViewModel { get; }
	}
}
