using System.Diagnostics;
using System.IO;
using System.Windows;
using YouTubePlaylistSyncer.WPF.ViewModel;
using System;

namespace YouTubePlaylistSyncer.WPF {
	/// <summary>
	/// Put anything that needs to be done on startup and on exit here.
	/// </summary>
	public partial class App : Application {

		protected override void OnStartup(StartupEventArgs e) {
			this.Precheck();

			MainWindow = new MainWindow() {
				DataContext = new MainPageViewModel()
			};
			MainWindow.Show();

			base.OnStartup(e);
		}

		protected override void OnExit(ExitEventArgs e) {
			base.OnExit(e);
		}

		private void Precheck() {
			if (Environment.GetEnvironmentVariable("GoogleAPIKey") is null) {
				MessageBox.Show("GoogleAPIKey not found in environment variables.  The program will now exit.", "API key not found", MessageBoxButton.OK, MessageBoxImage.Error);
				Environment.Exit(1);
			}
		}
	}
}
