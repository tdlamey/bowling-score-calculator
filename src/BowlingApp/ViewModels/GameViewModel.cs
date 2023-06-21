using BowlingApp.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace BowlingApp.ViewModels
{
	/// <summary>
	/// A view model for binding to the game view.
	/// </summary>
	public class GameViewModel : ViewModelBase<GameModel>
	{
		/// <summary>
		/// Creates a new instance of <see cref="GameViewModel"/>.
		/// </summary>
		public GameViewModel()
		{
			Model = new GameModel();

			FrameViewModels = new ObservableCollection<FrameViewModel>(Model.Frames.Select(frame => new FrameViewModel(frame)));
		}

		/// <summary>
		/// Creates a new instance of <see cref="GameViewModel"/>.
		/// </summary>
		/// <param name="scoreEntryViewModel">
		/// The view model for the score input view.
		/// </param>
		internal GameViewModel(ScoreEntryViewModel scoreEntryViewModel)
			: this()
		{
			scoreEntryViewModel.DeliveryValueAssigned += (s, e) => Model.OnDeliveryValueAssigned(e.Value);

			Model.CurrentDeliveryAvailableValues.CollectionChanged += (s, e) =>
			{
				scoreEntryViewModel.SetAvailableScoreEntries(Model.CurrentDeliveryAvailableValues);
			};

			scoreEntryViewModel.SetAvailableScoreEntries(Model.CurrentDeliveryAvailableValues);
		}

		/// <summary>
		/// A collection of view models for each frame of the game.
		/// </summary>
		public ObservableCollection<FrameViewModel> FrameViewModels { get; }
	}
}
