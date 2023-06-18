using BowlingApp.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace BowlingApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			KeyDown += (s, e) => PassKeyToScoreInput(e.Key);
		}

		private void PassKeyToScoreInput(Key key)
		{
			//Null value checking is not necessary here, considering the use of this method.
			// We'll include it anyway to show good practice.

			var mainViewModel = MainView?.DataContext as MainViewModel;

			mainViewModel?.ScoreEntryViewModel?.OnKeyPressed(key);
		}
	}
}
