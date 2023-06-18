using BowlingApp.Models;

namespace BowlingApp.ViewModels
{
	public class FrameShotViewModel : ViewModelBase<FrameShotModel>
	{
		public FrameShotViewModel()
		{
			if (IsInDesignMode)
			{
				Model = new FrameShotModel();
			}
		}

		internal FrameShotViewModel(FrameShotModel model)
		{
			Model = model;
		}
	}
}
