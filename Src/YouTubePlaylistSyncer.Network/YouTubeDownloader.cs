using MediaToolkit;
using MediaToolkit.Model;
using System.IO;
using System.Threading.Tasks;
using VideoLibrary;

namespace YouTubePlaylistSyncer.Network {
	public class YouTubeDownloader {
		private static YouTube youtube = YouTube.Default;
		public string OutputLocation { get; set; }

		/// <summary>
		/// Downloads the temp video file, converts it to the given destination filename, then deletes the temp file.
		/// </summary>
		public async Task DownloadAsync(string url, string destFilenameOnDisk) {
			YouTubeVideo video = await youtube.GetVideoAsync(url);
			string tempFullpath = $@"{OutputLocation}\TEMP.{video.FullName}";
			string destFullpath = $@"{OutputLocation}\{destFilenameOnDisk}";

			File.WriteAllBytes(tempFullpath, video.GetBytes());

			var srcFile = new MediaFile() { Filename = tempFullpath };
			var destFile = new MediaFile() { Filename = destFullpath };

			using (var engine = new Engine()) {
				engine.GetMetadata(srcFile);
				engine.Convert(srcFile, destFile);
			}

			File.Delete(tempFullpath);
		}
	}
}
