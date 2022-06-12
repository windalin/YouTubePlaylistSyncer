using System;
using System.Threading.Tasks;
using YouTubePlaylistSyncer.Network;
using System.IO;

namespace YouTubePlaylistSyncer.WPF.Model {
	public class Video : ModelBase {

		public delegate void DownloadFinishedEventHandler(Video sender);
		public event DownloadFinishedEventHandler DownloadFinished;

		public static readonly char[] InvalidCharacters = { '\\', '/', '*', ':', '?', '"', '<', '>', '|' }; // invalid in windows filenames

		public int Index { get; set; }
		public string Title { get; set; }
		public string ID { get; set; }
		public string Status { get; set; }
		public Duration Duration { get; set; }
		public string FilenameOnDisk { get; set; }
		public string AgeRestricted { get; set; }
		public string Action { get; set; }

		private string downloadStatus;
		public string DownloadStatus {
			get => this.downloadStatus;
			set {
				this.downloadStatus = value;
				OnPropertyChanged();
			}
		}
		public string NamingScheme { get; set; }

		public override string ToString() {
			return $"{Index}. {Title}\n{ID}, {Status}, {Duration}\nName on disk: [{FilenameOnDisk}]\n";
		}

		public void ApplyNamingScheme(string namingScheme) {
			if (namingScheme is "Remove Invalid Characters") {
				FilenameOnDisk = string.Join("", Title.Split(InvalidCharacters)) + ".mp3"; // TODO: add support for other file extensions
			}
		}

		public void SetRequiredAction(string outputLocation) {
			if (Status is not "public" || AgeRestricted is "ytAgeRestricted" || File.Exists($@"{outputLocation}\{FilenameOnDisk}")) {
				Action = "None";
			} else {
				Action = "Download";
			}
		}

		public async Task DownloadAsync(YouTubeDownloader downloader) {
			DownloadStatus = Model.DownloadStatus.Downloading;

			try {
				await downloader.DownloadAsync($"https://www.youtube.com/watch?v={ID}", FilenameOnDisk);
				DownloadStatus = Model.DownloadStatus.Completed;
			} catch (Exception e) {
				DownloadStatus = Model.DownloadStatus.Failed;
			}

			DownloadFinished?.Invoke(this);
		}
	}

	public class Duration {
		public string ISO8061DurationString { get; set; }
		public int Seconds { get; set; }

		public Duration(string iso8061DurationString) {
			ISO8061DurationString = iso8061DurationString;
			Seconds = (int)System.Xml.XmlConvert.ToTimeSpan(iso8061DurationString).TotalSeconds;
		}

		public override string ToString() {
			return $"[{ISO8061DurationString}, {Seconds}]";
		}
	}

	public class DownloadStatus {
		public const string Downloading = "Downloading...";
		public const string Completed = "Completed";
		public const string Failed = "Failed";
	}
}
