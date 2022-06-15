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

namespace UIAutomation.YouTubePlaylistSyncer.WPF.Grids {
	public class BaseGrid {

		private DataGridView grid;

		protected DataGridView Grid { 
			get => this.grid; 
			set {
				this.grid = value;
				this.ColumnIndices = this.GenerateColumnIndices();
			} 
		}
		protected DataGridViewRow[] Rows { get => Grid.Rows; }
		protected DataGridViewHeaderItem[] Columns { get => Grid.Header.Columns; }
		protected Dictionary<string, int> ColumnIndices;

		// Forcibly map column names to cell indices here since FlaUI doesn't support searching within a column
		private Dictionary<string, int> GenerateColumnIndices() {
			var dict = new Dictionary<string, int>();

			int i = 0;
			foreach (DataGridViewHeaderItem column in Columns) {
				dict[column.Name] = i;
				i++;
			}

			return dict;
		}
	}
}
