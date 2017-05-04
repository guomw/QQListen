﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Http;

namespace iQQ.Net.WebQQCore.Util.Extensions
{
    public static class HttpExtensions
    {
        //public static IEnumerable<Cookie> GetAllCookies(this CookieContainer cc)
        //{
        //    var table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
        //    BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance,
        //    null, cc, new object[] { });

        //    foreach (var pathList in table.Values)
        //    {
        //        var lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
        //        BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance,
        //        null, pathList, new object[] { });
        //        foreach (CookieCollection colCookies in lstCookieCol.Values)
        //        {
        //            foreach (var c in colCookies.OfType<Cookie>())
        //            {
        //                yield return c;
        //            }
        //        }
        //    }
        //}


        //public static IEnumerable<Cookie> GetCookies(this CookieContainer cc, string name)
        //{
        //    return GetAllCookies(cc).Where(item => string.Compare(item.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
        //}


        public static string GetRequestHeader(this QQHttpRequest request, CookieContainer cc)
        {
            var sb = new StringBuilder();
            sb.AppendLineIf($"{HttpConstants.Referrer}: {request.Referrer}", !request.Referrer.IsNullOrEmpty());
            sb.AppendLineIf($"{HttpConstants.UserAgent}: {request.UserAgent}", !request.UserAgent.IsNullOrEmpty());
            sb.AppendLineIf($"{HttpConstants.ContentType}: {request.ContentType}", !request.ContentType.IsNullOrEmpty());
            var cookies = cc.GetCookies(new Uri(request.Url)).OfType<Cookie>();
            sb.AppendLine($"{HttpConstants.Cookie}: {string.Join("; ", cookies)}");
            return sb.ToString();
        }

        public static bool IsNullOrEmpty(this CookieContainer col)
        {
            return col == null || col.Count == 0;
        }
    }
}
