using Microsoft.VisualStudio.TestTools.UnitTesting;
using YouTubePlaylistSyncer.Network;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System;

namespace UnitTest.YouTubePlaylistSyncer.Network {
	[TestClass]
	public class YouTubeDownloader_Tests {

		private const string invalidURL = "invalidURL";
		private const string invalidFilename = "invalidFilename.mp3";
		private readonly YouTubeDownloader downloader = new YouTubeDownloader() { 
			OutputLocation = Directory.GetCurrentDirectory()
		};

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task DownloadAsync_ThrowsException_For_InvalidVideoURL() {
			await downloader.DownloadAsync(invalidURL, invalidFilename);
		}

		[TestMethod]
		public async Task DownloadAsync_DoesNot_CreateOutputFile_For_InvalidVideoURL() {
			try {
				await downloader.DownloadAsync(invalidURL, invalidFilename);
			} catch {
				Assert.IsFalse(File.Exists($@"{this.downloader.OutputLocation}\{invalidFilename}"));
			}
		}
	}
}

