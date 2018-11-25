using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogFileCommunication {
	public class LogHandler {

		private const string FOLDER = "./Logs";

		public void WriteLog(List<string> data) {
			DoesFileExist();

			string todayDate = DateTime.Now.ToShortDateString();
			long currentTime = DateTime.Now.ToFileTimeUtc();

			string filePath = string.Format("{0}/{1}-{2}.txt", FOLDER, todayDate, currentTime);
			File.WriteAllLines(filePath, data.ToArray<string>());

		}

		public List<string> GetLogFiles() {
			DoesFileExist();

			return Directory.GetFiles(FOLDER, "*.txt")
				.Select(Path.GetFileName)
				.ToList();
		}

		public List<string> ReadLogFile(string fileName) {
			string path = string.Format("{0}/{1}", FOLDER, fileName);

			return File.ReadAllLines(path).ToList();
		}

		public bool DeleteLogFile(string fileName) {
			string path = string.Format("{0}/{1}", FOLDER, fileName);
			try {
				File.Delete(path);
				return true;
			} catch {
				return false;
			}
		}


		private static void DoesFileExist() {
			if (!Directory.Exists(FOLDER)) {
				Directory.CreateDirectory(FOLDER);
			}
		}
	}
}
