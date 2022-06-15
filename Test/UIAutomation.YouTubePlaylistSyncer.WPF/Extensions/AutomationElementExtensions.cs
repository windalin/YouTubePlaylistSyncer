using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlaUI.Core.Definitions;
using System.Runtime.CompilerServices;
using FlaUI.Core.Conditions;

namespace UIAutomation.YouTubePlaylistSyncer.WPF.Extensions {
	public static class AutomationElementExtensions {

		public static void AssertIsEnabled(this AutomationElement uiElement) {
			Assert.IsTrue(uiElement.IsEnabled, $"{uiElement.Name} is not enabled");
		}
	}
}
