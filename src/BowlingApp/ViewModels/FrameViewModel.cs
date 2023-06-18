using BowlingApp.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace BowlingApp.ViewModels
{
	public class FrameViewModel : ViewModelBase<FrameModel>
	{
		public FrameViewModel()
		{
			if (IsInDesignMode)
			{
				Model = new FrameModel
				{
					FrameScoreTotal = "150"
				};

				FrameShotViewModels = new ObservableCollection<FrameShotViewModel>
				{
					new FrameShotViewModel(),
					new FrameShotViewModel()
				};
			}
		}

		internal FrameViewModel(FrameModel frameModel)
		{
			Model = frameModel;

			FrameShotViewModels = new ObservableCollection<FrameShotViewModel>(Model.FrameShots.Select(frameShot => new FrameShotViewModel(frameShot)));
		}

		public ObservableCollection<FrameShotViewModel> FrameShotViewModels { get; }
	}
}
