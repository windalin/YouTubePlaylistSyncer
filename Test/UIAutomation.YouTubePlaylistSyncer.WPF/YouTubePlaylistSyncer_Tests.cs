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

namespace UIAutomation.YouTubePlaylistSyncer.WPF {

	[TestClass]
	public class YouTubePlaylistSyncer_Tests {

		private string debugApplicationPath = $@"{Directory.GetCurrentDirectory()}\..\..\..\..\..\Src\YouTubePlaylistSyncer.WPF\bin\Debug\net5.0-windows\YouTubePlaylistSyncer.WPF.exe";
		private Windows.YouTubePlaylistSyncer YouTubePlaylistSyncer { get; set; }

		[TestInitialize]
		public void TestInitialise() {
			Assert.IsTrue(File.Exists(debugApplicationPath), $"Application executable not found at {debugApplicationPath}, check config and build.");
			YouTubePlaylistSyncer = new Windows.YouTubePlaylistSyncer(debugApplicationPath);
		}

		[TestMethod]
		public void T1() {
		}
	}
}
