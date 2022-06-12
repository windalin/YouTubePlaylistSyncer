using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Threading.Thread;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using static System.Windows.Forms.SendKeys;
using FlaUI.Core.Conditions;
using Debug = System.Diagnostics.Debug;
using FlaUI.Core.Definitions;

namespace UIAutomation.YouTubePlaylistSyncer.WPF.Windows {
	public class WindowBase {
		protected Application application;

		protected ConditionFactory cf = new ConditionFactory(new UIA3PropertyLibrary());
		protected Window mainWindow;

		public WindowBase(string filepath) {
			application = Application.Launch(filepath);
			using (var automation = new UIA3Automation()) {
				mainWindow = application.GetMainWindow(automation);
			}
		}

		public void Close() => mainWindow.Close();
	}
}
