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
using UIAutomation.YouTubePlaylistSyncer.WPF.Extensions;
using UIAutomation.YouTubePlaylistSyncer.WPF.Grids;

namespace UIAutomation.YouTubePlaylistSyncer.WPF.Windows {
	public class YouTubePlaylistSyncer : WindowBase {

		private AutomationElement mainPageView;

		public Button GetRemotePlaylistInfoButton { get; }
		public Button BrowseButton { get; }
		public Button ApplyNamingSchemeButton { get; }
		public Button BeginDownloadButton { get; }
		public ComboBox FileNamingSchemeComboBox { get; }
		public RemotePlaylistDataGrid RemotePlaylistDatagrid { get; }
		public TextBox PlaylistURLTextBox { get; }
		public TextBox OutputLocationTextBox { get; }
		public TabItem RemotePlaylistTab { get; }

		public YouTubePlaylistSyncer(string filepath) : base(filepath) {
			this.mainPageView = mainWindow.FindFirstChild(cf.ByClassName("MainPageView"));
			GetRemotePlaylistInfoButton = this.GetElementByAutomationID("GetRemotePlaylistInfoButton").AsButton();
			BrowseButton = this.GetElementByAutomationID("BrowseButton").AsButton();
			ApplyNamingSchemeButton = this.GetElementByAutomationID("ApplyNamingSchemeButton").AsButton();
			BeginDownloadButton = this.GetElementByAutomationID("BeginDownloadButton").AsButton();
			FileNamingSchemeComboBox = this.GetElementByAutomationID("FileNamingSchemeComboBox").AsComboBox();
			RemotePlaylistDatagrid = new RemotePlaylistDataGrid(this.GetElementByAutomationID("RemotePlaylistDataGrid").AsDataGridView());
			PlaylistURLTextBox = this.GetElementByAutomationID("PlaylistURLTextBox").AsTextBox();
			OutputLocationTextBox = this.GetElementByAutomationID("OutputLocationTextBox").AsTextBox();
			RemotePlaylistTab = this.GetElementByAutomationID("RemotePlaylistTab").AsTabItem();
		}

		private AutomationElement GetElementByAutomationID(string automationID) {
			return this.mainPageView.FindFirstDescendant(cf.ByAutomationId(automationID));
		}

		public YouTubePlaylistSyncer EnterPlaylistURL(string url) {
			PlaylistURLTextBox.AssertIsEnabled();
			PlaylistURLTextBox.Enter(url);
			return this;
		}

		public YouTubePlaylistSyncer ClickGetRemotePlaylistInfo() {
			GetRemotePlaylistInfoButton.AssertIsEnabled();
			GetRemotePlaylistInfoButton.Click();
			return this;
		}

		public YouTubePlaylistSyncer EnterOutputLocation(string outputLocation) {
			OutputLocationTextBox.AssertIsEnabled();
			OutputLocationTextBox.Enter(outputLocation);
			return this;
		}

		public YouTubePlaylistSyncer ClickBrowse() {
			BrowseButton.AssertIsEnabled();
			BrowseButton.Click();
			return this;
		}

		public YouTubePlaylistSyncer SelectFileNamingScheme(string namingScheme) {
			FileNamingSchemeComboBox.AssertIsEnabled();
			int index = 0;
			switch (namingScheme) {
				case "Remove Invalid Characters":
					index = 0;
					break;
			}
			FileNamingSchemeComboBox.Select(index);
			FileNamingSchemeComboBox.SelectedItem.Click();
			return this;
		}

		public YouTubePlaylistSyncer ClickApplyNamingScheme() {
			ApplyNamingSchemeButton.AssertIsEnabled();
			ApplyNamingSchemeButton.Click();
			return this;
		}

		public YouTubePlaylistSyncer ClickBeginDownload() {
			BeginDownloadButton.AssertIsEnabled();
			BeginDownloadButton.Click();
			return this;
		}
	}
}
