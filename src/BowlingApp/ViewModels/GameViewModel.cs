using BowlingApp.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace BowlingApp.ViewModels
{
	public class GameViewModel : ViewModelBase<GameModel>
	{
		public GameViewModel()
		{
			Model = new GameModel();

			FrameViewModels = new ObservableCollection<FrameViewModel>(Model.Frames.Select(frame => new FrameViewModel(frame)));
		}

		internal GameViewModel(ScoreEntryViewModel scoreEntryViewModel)
			: this()
		{
			scoreEntryViewModel.ShotValueSelected += (s, e) => Model.OnShotValueAssigned(e.Value);

			Model.CurrentFrameShotAvailableValues.CollectionChanged += (s, e) =>
			{
				scoreEntryViewModel.SetAvailableShotValues(Model.CurrentFrameShotAvailableValues);
			};

			scoreEntryViewModel.SetAvailableShotValues(Model.CurrentFrameShotAvailableValues);
		}

		public ObservableCollection<FrameViewModel> FrameViewModels { get; }
	}
}
