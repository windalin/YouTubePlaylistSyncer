using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace YouTubePlaylistSyncer.WPF.Model {
	// TODO: decide whether or not this class is actually needed for remote/local difference building
	public class Playlist {
		public string ID { get; set; }

		// this will probably need to be changed to observablecollection
		public ObservableCollection<Video> Videos { get; set; } = new ObservableCollection<Video>();
		public int Pages => Videos.Count % 50 > 0 ? Videos.Count / 50 + 1 : Videos.Count / 50;
		public string OutputLocation { get; set; }

		/// <summary>
		/// Constructors can't be async, so I'm using this Build method that hits the API for the videos then constructs a Playlist out of the response.
		/// </summary>
		public static async Task<Playlist> BuildPlaylistAsync(string playlistID, string outputLocation) {

			return new Playlist() {
				ID = playlistID,
				Videos = new ObservableCollection<Video>() { new Video() { Index=-1, Title="video -1", ID="id -1" } },
				OutputLocation = outputLocation
			};
		}

	}
}
