using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaUI.Core;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UIAutomation.YouTubePlaylistSyncer.WPF.Windows;
using System.IO;
using System.Threading;

namespace UIAutomation.YouTubePlaylistSyncer.WPF {

	[TestClass]
	public class YouTubePlaylistSyncer_Tests {
		
		private string debugApplicationPath = $@"{Directory.GetCurrentDirectory()}\..\..\..\..\..\Src\YouTubePlaylistSyncer.WPF\bin\Debug\net5.0-windows\YouTubePlaylistSyncer.WPF.exe";
		private const string samplePlaylistURL = "https://www.youtube.com/playlist?list=PL0aLzmJXpfq7MXC1leFDYdLbj1-n-A8Ze";
		private const string outputLocation = @"C:\Users\Public\Downloads";
		private Windows.YouTubePlaylistSyncer YouTubePlaylistSyncer { get; set; }

		[TestInitialize]
		public void TestInitialise() {
			Assert.IsTrue(File.Exists(debugApplicationPath), $"Application executable not found at {debugApplicationPath}, check config and build.");
			YouTubePlaylistSyncer = new Windows.YouTubePlaylistSyncer(debugApplicationPath);
		}

		[TestCleanup]
		public void TestCleanup() {
			YouTubePlaylistSyncer.Close();
		}

		[TestMethod]
		[Description("The Get Remote Playlist Info button should be disabled by default, and enabled if a valid playlist URL is entered.")]
		public void GetRemotePlaylistInfo_Button_Status() {
			Assert.IsFalse(YouTubePlaylistSyncer.GetRemotePlaylistInfoButton.IsEnabled, "Remote Playlist button should be disabled.");
			YouTubePlaylistSyncer.EnterPlaylistURL(samplePlaylistURL);
			Assert.IsTrue(YouTubePlaylistSyncer.GetRemotePlaylistInfoButton.IsEnabled, "Remote Playlist button should be enabled.");
		}

		[TestMethod]
		[Description("The Begin Download button should be disabled by default, and enabled only if an output location is given and a naming naming scheme is applied.")]
		public void BeginDownload_Button_Status() {
			Assert.IsFalse(YouTubePlaylistSyncer.BeginDownloadButton.IsEnabled, "Begin Download button should be disabled.");
			YouTubePlaylistSyncer
				.EnterPlaylistURL(samplePlaylistURL)
				.ClickGetRemotePlaylistInfo();
			Thread.Sleep(3000); // TODO: need a better way of waiting for a condition
			YouTubePlaylistSyncer
				.EnterOutputLocation(outputLocation)
				.ClickApplyNamingScheme();
			Thread.Sleep(500);
			Assert.IsTrue(YouTubePlaylistSyncer.BeginDownloadButton.IsEnabled, "Begin Download button should be enabled.");
		}
	}
}
