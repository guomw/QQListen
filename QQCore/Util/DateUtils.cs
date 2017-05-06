﻿using System;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Util
{
    public class DateUtils
    {
        public static DateTime Parse(JObject jsonobj)
        {
            const string format = "yyyy-M-d";
            var dateString = jsonobj["year"] + "-" + jsonobj["month"] + "-" + jsonobj["day"];
            if (dateString.Equals("0-0-0"))
                dateString = "1970-1-1";
            var dt = DateTime.ParseExact(dateString, format, CultureInfo.CurrentCulture);
            return dt;


        }

        /// <summary>
        /// 自1970年1月1日0时起的毫秒数
        /// </summary>
        /// <returns></returns>
        public static long NowTimestamp()
        {
            return DateTime.Now.CurrentTimeMillis();
        }
    }

    public static class DateTimeExtensions
    {
        private static readonly DateTime _jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 自1970年1月1日0时起的毫秒数
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static long CurrentTimeMillis(this DateTime d)
        {
            return (long)((DateTime.UtcNow - _jan1St1970).TotalMilliseconds);
        }

        public static long CurrentTimeSeconds(this DateTime d)
        {
            return CurrentTimeMillis(d) / 1000;
        }
    }

}
