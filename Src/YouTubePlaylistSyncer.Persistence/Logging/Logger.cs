using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubePlaylistSyncer.Persistence.Logging {

	// singleton to make this basically a global logger than i can use anywhere
	public class Logger {

		private static Logger instance;
		private static readonly Guid guid = Guid.NewGuid();
		private static readonly string logFilePath = Directory.GetCurrentDirectory() + $@"\Log.txt"; // TODO: find a better place to put this log file

		private Logger() { }

		public void WriteToLogFile(string text, bool writeGuid = true) {
			string textOut = writeGuid ? $"\n{guid}\n{text}" : $"\n{text}";
			File.AppendAllText(logFilePath, textOut);
		}

		public static Logger CurrentInstance {
			get {
				if (instance is null) {
					instance = new Logger();
				}
				return instance;
			}
		}
	}
}
