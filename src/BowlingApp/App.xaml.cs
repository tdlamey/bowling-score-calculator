using System.Windows;
using System.Windows.Threading;

namespace BowlingApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			(MainWindow as MainWindow)?.HandleExceptionGlobal(e);
		}
	}
}
