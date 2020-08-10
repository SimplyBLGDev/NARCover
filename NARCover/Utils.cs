using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NARCover {
	public class Utils {

		public static string GetSimplifiedGameName(string name) {
			string r = "";
			int parentheses = 0;
			int brackets = 0;
			int chevrons = 0;

			for (int i = 0; i < name.Length; i++) {
				switch (name[i]) {
					case '(': parentheses++; break;
					case '[': brackets++; break;
					case '<': chevrons++; break;
				}

				if (parentheses <= 0 && brackets <= 0 && chevrons <= 0)
					r += name[i];

				switch (name[i]) {
					case ')': parentheses--; break;
					case ']': brackets--; break;
					case '>': chevrons--; break;
				}
			} // Remove anything between parentheses, brackets or chevrons

			for (int i = 0; i < r.Length - 1; i++) {
				// Titles starting with 'The' are usually rewritten with ', The' at the end to better sort their names, this re-inserts that ending
				//  into the beggining this method generalizes any such occurences
				if (r[i] == ',' && r[i + 1] == ' ') {
					r = r.Substring(i + 2, r.Length - (i + 2)).Trim() + " " + r.Substring(0, i);
					break;
				}
			}

			r = r.Trim();

			return r;
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
		public string name;
		public string imageAddress;

		public GameInfo(string name) {
			this.name = name;
		}
	}

	public class APIException : Exception {
		public int errorCode;

		public APIException(string message, int code) : base(message) {
			errorCode = code;
		}
	}

	public class GameNotFoundException : Exception {
		public string game;

		public GameNotFoundException(string message, string game) : base(message) {
			this.game = game;
		}
	}
}
