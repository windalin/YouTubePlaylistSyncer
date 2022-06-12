using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YouTubePlaylistSyncer.WPF.Model;

namespace UnitTest.YouTubePlaylistSyncer.WPF.Model_Tests {
	[TestClass]
	public class Video_Tests {
		private readonly char[] invalidCharacters = { '\\', '/', '*', ':', '?', '"', '<', '>', '|' }; // invalid in windows filenames
		private const string invalidTitle = "Invalid Title \\ / * : ? \" < > |";

		[TestMethod]
		public void ApplyNamingScheme_Removes_All_InvalidCharacters() {
			var video = new Video() {
				Title = invalidTitle
			};
			video.ApplyNamingScheme("Remove Invalid Characters");

			foreach (char invalidChar in invalidCharacters) {
				Assert.IsFalse(video.FilenameOnDisk.Contains(invalidChar), $"Expected FilenameOnDisk to not contain {invalidChar}.");
			}
		}
	}
}
