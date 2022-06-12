using Microsoft.VisualStudio.TestTools.UnitTesting;
using YouTubePlaylistSyncer.Network;
using System.Diagnostics;
using System.Threading.Tasks;
using Google;

namespace UnitTest.YouTubePlaylistSyncer.Network {

	[TestClass]
	public class YouTubeAPI_Tests {

		private const string invalidPlaylistID = "invalidPlaylistID";

		[TestMethod]
		[ExpectedException(typeof(GoogleApiException))]
		public async Task GetPlaylistAsync_ThrowsException_For_InvalidPlaylistID() {
			var resp = await YouTubeAPI.GetPlaylistPageAsync(invalidPlaylistID);
		}

		[TestMethod]
		public async Task GetVideoDurationPageAsync_ReturnsZeroResults_For_InvalidVideoIDs() {
			var resp = await YouTubeAPI.GetVideoDurationPageAsync("invalidID1,invalidID2,invalidID3");
			Assert.AreEqual(0, resp.PageInfo.TotalResults, $"Expected 0 results but got {resp.PageInfo.TotalResults} results.");
		}
	}
}
