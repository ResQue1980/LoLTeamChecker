﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FluorineFx.AMF3;
using FluorineFx.Messaging.Messages;
using FluorineFx.Messaging.Rtmp.Event;
using LoLTeamChecker.Messaging.Messages;

namespace LoLTeamChecker.Util
{
	public static class RtmpUtil
	{
		[ThreadStatic]
		static Random _rand;
		static Random Rand { get { return _rand ?? (_rand = new Random()); }
		}

		public static bool IsError(Notify notify)
		{
			return notify.ServiceCall != null &&  notify.ServiceCall.ServiceMethodName == "_error";
		}
		public static bool IsResult(Notify notify)
		{
			return notify.ServiceCall != null &&
				(notify.ServiceCall.ServiceMethodName == "_result" || notify.ServiceCall.ServiceMethodName == "_error");
		}

		public static string FromByteArray(ByteArray obj)
		{
			if (obj == null)
				return null;

			var bytes = obj.MemoryStream.ToArray();
			if (bytes.Length < 16)
				return null;

			var ret = new StringBuilder();
			for (int i = 0; i < bytes.Length; i++)
			{
				if (i == 4 || i == 6 || i == 8 || i == 10)
				{
					ret.Append('-');
				}
				ret.AppendFormat("{0:X2}", bytes[i]);
			}

			return ret.ToString();
		}

		public static ByteArray ToByteArray(string str)
		{
			if (!IsUid(str))
				return null;

			str = str.Replace("-", "");

			var ret = new ByteArray();
			for (int i = 0; i < str.Length; i += 2)
			{
				byte num;
				if (!byte.TryParse(str.Substring(i, 2), NumberStyles.HexNumber, null, out num))
					return null;
				ret.WriteByte(num);
			}
			ret.Position = 0;
			return ret;
		}

		public static string RandomUidString()
		{
			return FromByteArray(RandomUid());
		}

		public static ByteArray RandomUid()
		{
			var arr = new byte[16];
			Rand.NextBytes(arr);
			return new ByteArray(arr);
		}

		public static bool IsUid(string str)
		{
			if (str != null && str.Length == 36)
			{
				int idx = 0;
				while (idx < 36)
				{

					var c = str[idx];
					if (idx == 8 || idx == 13 || idx == 18 || idx == 23)
					{
						if (c != 45)
						{
							return false;
						}
					}
					else if (c < 48 || c > 70 || c > 57 && c < 65)
					{
						return false;
					}
					idx++;
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// Gets the bodies of all the message arguments.
		/// </summary>
		/// <param name="notify"></param>
		/// <returns>Tuple, Item1 = Body, Item2 = TimeStamp</returns>
		public static IEnumerable<Tuple<object, Int64>> GetBodies(Notify notify)
		{
			var ret = new List<Tuple<object, Int64>>();
			foreach (var arg in notify.ServiceCall.Arguments)
			{
				Tuple<object, Int64> obj = null;
				if (arg is AbstractMessage)
				{
					var msg = (AbstractMessage)arg;
					obj = Tuple.Create(msg.Body, msg.TimeStamp);
				}
				else if (arg is MessageBase)
				{
					var msg = (MessageBase)arg;
					obj = Tuple.Create(msg.body, msg.timestamp);
				}

				if (obj != null)
					ret.Add(obj);
			}
			return ret;
		}
		
		/// <summary>
		/// Gets the error message from the notify arguments.
		/// </summary>
		/// <param name="notify"></param>
		/// <returns></returns>
		public static ErrorMessage GetError(Notify notify)
		{
			foreach (var arg in notify.ServiceCall.Arguments)
			{
				if (arg is ErrorMessage)
					return (ErrorMessage)arg;
			}
			return null;
		}
	}
}
