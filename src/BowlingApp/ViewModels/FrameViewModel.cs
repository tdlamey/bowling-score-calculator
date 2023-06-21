using BowlingApp.Models;
using System.Linq;

namespace BowlingApp.ViewModels
{
	/// <summary>
	/// A view model for binding to the frame view.
	/// </summary>
	public class FrameViewModel : ViewModelBase<FrameModel>
	{
		/// <summary>
		/// Creates a new instance of <see cref="FrameViewModel"/>.
		/// </summary>
		public FrameViewModel()
		{
			if (IsInDesignMode)
			{
				Model = new FrameModel
				{
					FrameScoreTotal = "150"
				};
				Model.Deliveries.First().Value = DeliveryValue.Strike;
				Model.UpdateDeliveryDisplay();
			}
		}

		/// <summary>
		/// Creates a new instance of <see cref="FrameViewModel"/>.
		/// </summary>
		/// <param name="frameModel">
		/// The model backing this view model.
		/// </param>
		internal FrameViewModel(FrameModel frameModel)
		{
			Model = frameModel;
		}
	}
}
