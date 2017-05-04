﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Util.Extensions
{
    public static class ObjectExtensions
    {
        public static T GetOrDefault<T>(this T obj, T defaultValue)
        {
            return obj == null ? defaultValue : obj;
        }

        public static bool IsDefault<T>(this T obj)
        {
            return obj.Equals(default(T));
        }

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool IsNullOrDefault<T>(this T? obj) where T :struct 
        {
            return obj == null || obj.Value.Equals(default(T));
        }
    }
}

