using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace YouTubePlaylistSyncer.WPF.Model {
	public class ModelBase : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
