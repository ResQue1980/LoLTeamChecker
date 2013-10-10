using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace LoLTeamChecker.Assets
{
	public class ChampIcons
	{
        const string ChampPath = @"C:\Riot Games\League of Legends\RADS\projects\lol_air_client\releases\";
        const string EndChampPath = @"\deploy\assets\images\champions\";
		static readonly object _sync;
		static readonly Dictionary<int, Bitmap> _cache;
		static Bitmap _unknown;


		static ChampIcons()
		{
			_sync = new object();
			_cache = new Dictionary<int, Bitmap>();
			//_unknown = SafeBitmap(ImagePath + "unknown.png");
		}


		static void AddCached(int key, Bitmap bmp)
		{
			lock (_sync)
			{
				_cache[key] = bmp;
			}
		}
		static Bitmap FindCached(int key)
		{
			lock (_sync)
			{
				Bitmap ret;
				return _cache.TryGetValue(key, out ret) ? ret : null;
			}
		}

		static Bitmap SafeBitmap(string file)
		{
			try
			{
				return File.Exists(file) ? new Bitmap(file) : null;
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public static Bitmap Get(int key)
		{
			var name = ChampNames.GetOrDefault(key);
			if (name == null)
			{
				return _unknown;
			}

			var bmp = FindCached(key);
			if (bmp != null)
				return bmp;
            String fullPath;
            String releaseFolder = System.IO.Directory.EnumerateDirectories(ChampPath).OrderBy(f => new DirectoryInfo(f).CreationTime).Last();
            fullPath = releaseFolder + EndChampPath;
            bmp = SafeBitmap(string.Format("{0}{1}_Square_0.png", fullPath, name));
			if (bmp == null)
			{
				return _unknown;
			}
			AddCached(key, bmp);
			return bmp;
		}
	}
}
