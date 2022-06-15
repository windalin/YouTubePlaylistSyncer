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

namespace UIAutomation.YouTubePlaylistSyncer.WPF.Grids {
	public class RemotePlaylistDataGrid : BaseGrid {

		public RemotePlaylistDataGrid(DataGridView grid) {
			Grid = grid;
		}

		public RemotePlaylistDataGrid T() {

			foreach (var item in this.ColumnIndices) {
				Debug.WriteLine($"colname is {item.Key}, index is {item.Value}");
			}

			return this;
		}

		public RemotePlaylistDataGrid SearchGrid(string column, string value) {
			int i = this.ColumnIndices[column];

			foreach (var row in Rows) {
				var cell = row.Cells[i];
				if (cell.Value == value) {
					cell.Click();
					return this;
				}
			}
			Assert.Fail($"{value} not found in {column}.");
			return this;
		}

	}
}
