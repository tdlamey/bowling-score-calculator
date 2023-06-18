namespace BowlingApp.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		public MainViewModel()
		{
			ScoreEntryViewModel = new ScoreEntryViewModel();
			GameViewModel = new GameViewModel(ScoreEntryViewModel);
		}

		public ScoreEntryViewModel ScoreEntryViewModel { get; }

		public GameViewModel GameViewModel { get; }
	}
}
