using BowlingApp.Models;
using System.ComponentModel;
using System.Windows;

namespace BowlingApp.ViewModels
{
	/// <summary>
	/// A base class for all view models.
	/// </summary>
	public abstract class ViewModelBase : NotifyPropertyChangedBase
	{
		/// <summary>
		/// Determines whether the class is being used in the Visual Studio designer view.
		/// </summary>
		protected static bool IsInDesignMode
			=> DesignerProperties.GetIsInDesignMode(new DependencyObject());

	}

	/// <summary>
	/// A base class for all view models with an corresponding model.
	/// </summary>
	/// <typeparam name="TModel">
	/// The type of the model
	/// </typeparam>
	public abstract class ViewModelBase<TModel> : ViewModelBase
		where TModel : ModelBase
	{
		/// <summary>
		/// The model backing the current view model.
		/// </summary>
		public TModel Model { get; protected set; }
	}
}
