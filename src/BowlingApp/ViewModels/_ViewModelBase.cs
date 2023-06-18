using BowlingApp.Models;

namespace BowlingApp.ViewModels
{
	public abstract class ViewModelBase : NotifyPropertyChangedBase { }

	public abstract class ViewModelBase<TModel> : ViewModelBase
		where TModel : ModelBase
	{
		public TModel Model { get; protected set; }
	}
}
