using BowlingApp.Models;
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
				Model.Deliveries.First().Value = ShotValue.Strike;
				Model.UpdateDeliveryDisplay();
			}
		}

		internal FrameViewModel(FrameModel frameModel)
		{
			Model = frameModel;
		}
	}
}
