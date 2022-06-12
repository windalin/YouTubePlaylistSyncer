using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using YouTubePlaylistSyncer.WPF.ViewModel;

namespace YouTubePlaylistSyncer.WPF.Command {
	public abstract class CommandBase : ICommand {
		protected readonly MainPageViewModel viewModel;

		public CommandBase(MainPageViewModel viewModel) {
			this.viewModel = viewModel;
		}


		public event EventHandler CanExecuteChanged;

		public virtual bool CanExecute(object parameter) {
			return true;
		}

		public abstract void Execute(object parameter);

		protected void OnCanExecuteChanged() {
			CanExecuteChanged?.Invoke(this, new EventArgs());
		}
	}
}
