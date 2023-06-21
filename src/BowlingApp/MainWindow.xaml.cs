using BowlingApp.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace BowlingApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		/// Creates a new instance of <see cref="MainWindow"/>.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Occurs when initialization of the user control is complete.
		/// </summary>
		/// <param name="e">
		/// Event information.
		/// </param>
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			KeyDown += (s, e) => PassKeyToScoreInput(e.Key);
		}

		/// <summary>
		/// Passes the keyboard input to the score input view model for processing.
		/// </summary>
		/// <param name="key">
		/// The key that was pressed.
		/// </param>
		/// <remarks>
		/// Attaching keyboard input at the window level allows the keys to act as
		/// shortcuts for score input regardless of which UI element, if any, currently has focus.
		/// </remarks>
		private void PassKeyToScoreInput(Key key)
		{
			//Null value checking is not necessary here, considering the use of this method.
			// We'll include it anyway to show good practice.

			var mainViewModel = MainView?.DataContext as MainViewModel;

			mainViewModel?.ScoreEntryViewModel?.OnKeyPressed(key);
		}

		internal void HandleExceptionGlobal(DispatcherUnhandledExceptionEventArgs e)
		{
			Dispatcher.CheckAndInvoke(() =>
			{
				MessageBox.Show(e.Exception.Message, "An error has occurred", MessageBoxButton.OK, MessageBoxImage.Error);
			});

			e.Handled = true;
		}
	}
}
