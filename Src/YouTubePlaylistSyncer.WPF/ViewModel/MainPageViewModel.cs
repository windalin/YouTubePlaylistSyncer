using Google.Apis.YouTube.v3.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using YouTubePlaylistSyncer.Network;
using YouTubePlaylistSyncer.WPF.Command;
using YouTubePlaylistSyncer.WPF.Model;
using Video = YouTubePlaylistSyncer.WPF.Model.Video;
using System.Collections.Generic;

namespace YouTubePlaylistSyncer.WPF.ViewModel {
	public class MainPageViewModel : ViewModelBase {

		#region Fields not tied to properties
		private YouTubeDownloader downloader;

		/// <summary>
		/// Limits the number of videos that can be in the downloading process at any given time, also serves as the index for the next video ready to be downloaded.  Each time a video finishes downloading, 
		/// start the download for the next video with index equal to limit and atomically increment limit.  Probably needs a better name.  The limit is finicky, seems to be around 40-50 and existing open videos
		/// e.g., video open in a browser also seems to be counted in this, thus 30 is used for now.
		/// </summary>
		private int limit = 30;
		private readonly object limitLock = new object();

		/// <summary>
		/// Not all videos in the remote playlist should be downloaded.
		/// </summary>
		private List<Video> videosToDownload;
		#endregion

		#region Constructor
		public MainPageViewModel() {
			GetRemotePlaylistCommand = new GetRemotePlaylistCommand(this);
			BrowseOutputLocationCommand = new BrowseOutputLocationCommand(this);
			ApplyNamingSchemeCommand = new ApplyNamingSchemeCommand(this);
			BeginDownloadCommand = new BeginDownloadCommand(this);
			TestCommand = new TestCommand(this);
		}
		#endregion

		#region Properties
		private string playlistUrl = "";
		public string PlaylistURL {
			get => this.playlistUrl;
			set {
				this.playlistUrl = value;
				PlaylistID = this.playlistUrl.Split("=").LastOrDefault();
				OnPropertyChanged();
			}
		}

		private string playlistID = "";
		public string PlaylistID {
			get => this.playlistID;
			private set => this.playlistID = value;
		}

		private string outputLocation = "";
		public string OutputLocation {
			get => this.outputLocation;
			set {
				this.outputLocation = value;
				OnPropertyChanged();
			}
		}

		public string[] NamingSchemesArray { get; } = { "Remove Invalid Characters" }; // TODO: add more naming schemes

		private string namingScheme = "Remove Invalid Characters";
		public string NamingScheme {
			get => this.namingScheme;
			set {
				this.namingScheme = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<Video> remotePlaylistVideos = new ObservableCollection<Video>();
		public ObservableCollection<Video> RemotePlaylistVideos { get => this.remotePlaylistVideos; }

		// TODO: local playlist and difference building
		#endregion

		#region Methods
		/// <summary>
		/// Whenever a property of interest is changed in any of the remote playlist's videos, refresh the remote playlist datagrid.
		/// </summary>
		public void RemotePlaylistItemPropertyChanged(object sender, PropertyChangedEventArgs e) => this.RefreshRemotePlaylistGrid();

		/// <summary>
		/// Request for all the videos, then request again for the duration and age restricted rating of each video.
		/// </summary>
		public async void GetRemotePlaylistInfo() {
			string nextPageToken = null;
			int i = 1;

			do {
				PlaylistItemListResponse resp = await YouTubeAPI.GetPlaylistPageAsync(this.playlistID, nextPageToken); // TODO: request may throw exception
				nextPageToken = resp.NextPageToken;

				foreach (PlaylistItem item in resp.Items) {
					var video = new Video() {
						Index = i++,
						Title = item.Snippet.Title,
						ID = item.ContentDetails.VideoId,
						Status = item.Status.PrivacyStatus
					};
					video.PropertyChanged += RemotePlaylistItemPropertyChanged;
					this.remotePlaylistVideos.Add(video);
				}

			} while (nextPageToken is not null);

			int pages = (this.remotePlaylistVideos.Count % 50 > 0) ? (this.remotePlaylistVideos.Count / 50 + 1) : (this.remotePlaylistVideos.Count / 50);
			int k = 0;
			for (int j = 0; j < pages; j++) {
				string IDs = string.Join(",", this.remotePlaylistVideos.Skip(j * 50).Take(50).Select(x => x.ID));
				VideoListResponse resp = await YouTubeAPI.GetVideoDurationPageAsync(IDs);

				foreach (Google.Apis.YouTube.v3.Data.Video item in resp.Items) {
					this.remotePlaylistVideos[k].Duration = new Duration(item.ContentDetails.Duration);
					this.remotePlaylistVideos[k].AgeRestricted = item.ContentDetails.ContentRating.YtRating; // TODO: yt api doesn't work properly, sometimes it returns nothing even if a video is age restricted, need an alternative
					k++;
				}
			}

			this.RefreshRemotePlaylistGrid();
		}

		public void OpenFolderDialogAndSetOutputLocation() {
			using (var dialog = new FolderBrowserDialog()) {
				dialog.ShowDialog();
				OutputLocation = dialog.SelectedPath;
			}
		}

		/// <summary>
		/// Applies the chosen naming scheme to each video, then check outputLocation for files matching the given naming scheme to decide on the required action.
		/// </summary>
		public void ApplyNamingScheme() {
			foreach (Video video in this.remotePlaylistVideos) {
				video.ApplyNamingScheme(NamingScheme);
				video.SetRequiredAction(this.outputLocation);
			}
			this.RefreshRemotePlaylistGrid();
			OnPropertyChanged();
		}

		/// <summary>
		/// Start a limited number of concurrent downloads.  Each time a download finishes, a remaining download is started, if any.
		/// </summary>
		public void BeginDownload() {
			this.videosToDownload = this.remotePlaylistVideos.Where(x => x.Action is "Download").ToList();

			this.downloader = new YouTubeDownloader() {
				OutputLocation = this.outputLocation
			};

			foreach (Video video in this.videosToDownload.Take(this.limit)) {
				video.DownloadFinished += this.StartNextDownload;
				Task.Run(() => video.DownloadAsync(this.downloader));
			}
		}

		/// <summary>
		/// Videos raise the DownloadFinished event which in turn calls this method, requires a lock on the index of the video to download.
		/// </summary>
		/// <param name="sender"></param>
		private void StartNextDownload(Video sender) {
			if (this.limit < this.videosToDownload.Count) {
				lock (this.limitLock) {
					Video video = this.videosToDownload[this.limit];
					video.DownloadFinished += this.StartNextDownload;
					Task.Run(() => video.DownloadAsync(this.downloader));
					this.limit++;
				}
			}
		}

		public void RefreshRemotePlaylistGrid() => CollectionViewSource.GetDefaultView(RemotePlaylistVideos).Refresh();
		#endregion

		#region Commands
		public ICommand GetRemotePlaylistCommand { get; }
		public ICommand BrowseOutputLocationCommand { get; }
		public ICommand ApplyNamingSchemeCommand { get; }
		public ICommand BeginDownloadCommand { get; }
		public ICommand TestCommand { get; }
		#endregion
	}
}
