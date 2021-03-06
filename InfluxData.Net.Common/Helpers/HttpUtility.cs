﻿using System;

namespace InfluxData.Net.Common.Helpers
{
    /// <summary>
    /// Http utility methods.
    /// </summary>
    public static class HttpUtility
    {
        /// <summary>
        /// Escapes the string and makes it ready for URL usage. 
        /// </summary>
        /// <param name="value">Value to encode.</param>
        /// <returns>Encoded value.</returns>
        public static string UrlEncode(string value)
        {
            return Uri.EscapeUriString(value);
        }
    }
}