using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;


namespace LoLTeamChecker.Util
{
    //Using these methods for the exceptions
    public static class Parse
    {
        public static int Int(string str)
        {
            if (str == "NaN")
                return 0;

            int ret;
            if (int.TryParse(str, out ret))
                return ret;

            throw new FormatException(string.Format("Expected {0} got {1}", ret.GetType(), str));
        }
        public static long Long(string str)
        {
            if (str == "NaN")
                return 0;

            long ret;
            if (long.TryParse(str, out ret))
                return ret;

            throw new FormatException(string.Format("Expected {0} got {1}", ret.GetType(), str));
        }
        public static bool Bool(string str)
        {
            bool ret;
            if (bool.TryParse(str, out ret))
                return ret;

            throw new FormatException(string.Format("Expected {0} got {1}", ret.GetType(), str));
        }
        /// <summary>
        /// Parses dates in the following format "Sat Oct 22 22:52:44 GMT-0400 2011"
        /// </summary>
        /// <param name="str">Date formated as "Sat Oct 22 22:52:44 GMT-0400 2011"</param>
        /// <returns>DateTime Utc</returns>
        /// <exception cref="FormatException">Throws when it is unable to match and parse date.</exception>
        public static DateTime Date(string str)
        {
            var match = Regex.Match(str, @"\w+ (?<month>\w+) (?<day>\d+) (?<hour>\d+):(?<minute>\d+):(?<second>\d+) GMT(?<gmt>[+-]\d\d)\d* (?<year>\d+)");
            if (!match.Success)
                throw new FormatException("Invalid date format " + str);

            string date = string.Format("{0} {1} {2} {3} {4} {5} {6}", match.Groups["month"], match.Groups["day"], match.Groups["hour"], match.Groups["minute"], match.Groups["second"], match.Groups["gmt"], match.Groups["year"]);

            DateTime ret;
            if (!DateTime.TryParseExact(date, "MMM dd HH mm ss zz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out ret))
                throw new FormatException("Invalid date format " + str);

            return ret;
        }

		public static string ToBase64(string str)
		{
			if (str == null)
				return null;
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
		}
    }
}
