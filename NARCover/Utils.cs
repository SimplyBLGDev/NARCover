using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NARCover {
	public static class Utils {

		public static string GetSimplifiedGameName(string name) {
			string r = name;

			r = RemoveID(r);
			r = RemoveParentheses(r);
			r = RemoveVersion(r);
			r = RelocateAfterComma(r);
			r = r.Trim(); // Remove whitespace

			return r;
		}

		// Remove anything between parentheses, brackets and chevrons
		public static string RemoveParentheses(string from) {
			string ret = "";

			int parentheses = 0;
			int brackets = 0;
			int chevrons = 0;

			for (int i = 0; i < from.Length; i++) {
				switch (from[i]) {
					case '(': parentheses++; break;
					case '[': brackets++; break;
					case '<': chevrons++; break;
				}

				if (parentheses <= 0 && brackets <= 0 && chevrons <= 0)
					ret += from[i];

				switch (from[i]) {
					case ')': parentheses--; break;
					case ']': brackets--; break;
					case '>': chevrons--; break;
				}
			}

			return ret;
		}

		// Remove any version identification found in the input string
		public static string RemoveVersion(string from) {
			string ret = from;

			foreach (Match m in Regex.Matches(from, " ?(([vV]ersion)|([vV]er)|[vV]) ?[0-9,\\-._]{1,}"))
				if (m.Success)
					ret = ret.Replace(m.Value, "");

			return ret;
		}

		// Remove the ID number at the beggining of the string ex: #### - Game Name would return 'Game name'
		public static string RemoveID(string from) {
			string ret = from;

			Match m = Regex.Match(from, "^[0 - 9]{ 1,} ( ?[-_:]) ? ?");

			if (m.Success)
				ret = ret.Replace(m.Value, "");

			return ret;
		}

		// Titles starting with 'The' are usually rewritten with ', The' at the end to better sort their names,
		// this re-inserts that ending into the beggining
		public static string RelocateAfterComma(string from) {
			string r = from;

			for (int i = 0; i < r.Length - 1; i++) {
				if (r[i] == ',' && r[i + 1] == ' ') {
					r = r.Substring(i + 2, r.Length - (i + 2)).Trim() + " " + r.Substring(0, i);
					break;
				}
			}

			return r;
		}

		public static string[] GetValidFiles(string romsPath, List<string> extensions, bool useFolderName, bool searchSubdirs) {
			List<string> gameFiles = new List<string>();

			if (useFolderName) {
				string[] names = Directory.GetDirectories(romsPath);

				foreach (string name in names)
					gameFiles.Add(Path.GetFileName(name)); // Folder name excluding path
			} else {
				SearchOption searchOption = searchSubdirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
				string[] names = Directory.GetFiles(romsPath, "*", searchOption);

				foreach (string name in names)
					if (extensions.Contains(Path.GetExtension(name)))
						gameFiles.Add(Path.GetFileNameWithoutExtension(name));
			}

			return gameFiles.ToArray();
		}

		public static string Get(string uri) {
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			using (Stream stream = response.GetResponseStream())
			using (StreamReader reader = new StreamReader(stream)) {
				return reader.ReadToEnd();
			}
		}
	}

	public class GameInfo {
		public string sourceFile;
		public string name;
		public string imageAddress;
		public string imageSaveAddress;

		public GameInfo(string name, string filename) {
			this.name = name;
			this.sourceFile = filename;
			imageAddress = "";
		}
	}

	public class APIException : Exception {
		public int errorCode;

		public APIException(int code) {
			errorCode = code;
		}
	}

	public class GameNotFoundException : Exception {
		public GameInfo game;

		public GameNotFoundException(GameInfo game) {
			this.game = game;
		}
	}
}
