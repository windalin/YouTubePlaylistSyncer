using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Threading.Tasks;

namespace YouTubePlaylistSyncer.Network {
	/// <summary>
	/// Wrap YouTubeService requests in 1 method.
	/// </summary>
	public static class YouTubeAPI {

		private const int MAX_RESULTS = 50;
		private static YouTubeService service = new YouTubeService(new BaseClientService.Initializer() { ApiKey = Environment.GetEnvironmentVariable("GoogleAPIKey") });

		/// <summary>
		/// Gets a single page of a playlist, given its ID and the pageToken.
		/// </summary>
		public static async Task<PlaylistItemListResponse> GetPlaylistPageAsync(string playlistID, string pageToken = null) {
			PlaylistItemsResource.ListRequest req = service.PlaylistItems.List("snippet,contentDetails,status");
			req.PlaylistId = playlistID;
			req.MaxResults = MAX_RESULTS;
			req.PageToken = pageToken;
			return await req.ExecuteAsync();
		}

		/// <summary>
		/// Gets contentDetails of a list of videos, should really be named GetContentDetails page or similar.
		/// </summary>
		public static async Task<VideoListResponse> GetVideoDurationPageAsync(string IDs) {
			VideosResource.ListRequest req = service.Videos.List("contentDetails");
			req.Id = IDs;
			return await req.ExecuteAsync();
		}
	}
}
