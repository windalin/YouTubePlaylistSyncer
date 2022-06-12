using System.ComponentModel;
using YouTubePlaylistSyncer.WPF.ViewModel;
using Video = YouTubePlaylistSyncer.WPF.Model.Video;

namespace YouTubePlaylistSyncer.WPF.Command {
	public class GetRemotePlaylistCommand : CommandBase {
		public GetRemotePlaylistCommand(MainPageViewModel viewModel) : base(viewModel) {
			this.viewModel.PropertyChanged += this.OnViewModelPropertyChanged;
		}

		/// <summary>
		/// If the PlaylistURL property changes in the viewmodel, check if the Get Remote Playlist Info button should be enabled.
		/// </summary>
		private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName is nameof(this.viewModel.PlaylistURL)) { this.OnCanExecuteChanged(); }
		}

		/// <summary>
		/// Naive check for a valid YouTube playlist URL.
		/// </summary>
		public override bool CanExecute(object parameter) => this.viewModel.PlaylistURL.Contains("youtube.com/playlist?list=") && this.viewModel.PlaylistID != string.Empty;

		public override void Execute(object parameter) {
			this.viewModel.RemotePlaylistVideos.Clear();
			this.viewModel.GetRemotePlaylistInfo();
		}
	}

	public class BrowseOutputLocationCommand : CommandBase {
		public BrowseOutputLocationCommand(MainPageViewModel viewModel) : base(viewModel) { }
		public override void Execute(object parameter) {
			this.viewModel.OpenFolderDialogAndSetOutputLocation();
		}
	}

	public class ApplyNamingSchemeCommand : CommandBase {
		public ApplyNamingSchemeCommand(MainPageViewModel viewModel) : base(viewModel) { }
		public override void Execute(object parameter) {
			this.viewModel.ApplyNamingScheme();
		}
	}

	public class BeginDownloadCommand : CommandBase {
		public BeginDownloadCommand(MainPageViewModel viewModel) : base(viewModel) {
			this.viewModel.PropertyChanged += this.OnViewModelPropertyChanged;
		}

		private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName is "ApplyNamingScheme" or nameof(this.viewModel.OutputLocation)) { this.OnCanExecuteChanged(); }
		}

		public override bool CanExecute(object parameter) {
			if (this.viewModel.RemotePlaylistVideos.Count == 0 || string.IsNullOrEmpty(this.viewModel.OutputLocation)) { return false; }

			foreach (Video video in this.viewModel.RemotePlaylistVideos) {
				if (string.IsNullOrEmpty(video.FilenameOnDisk)) { return false; }
			}
			return true;
		}

		public override void Execute(object parameter) {
			this.viewModel.BeginDownload();
		}
	}

	public class TestCommand : CommandBase {
		public TestCommand(MainPageViewModel viewModel) : base(viewModel) { }

		public override void Execute(object parameter) {
		}
	}
}