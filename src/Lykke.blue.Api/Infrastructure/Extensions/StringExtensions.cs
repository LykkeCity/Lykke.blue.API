﻿using System.Collections.Generic;
using System.Linq;

namespace Lykke.blue.Api.Infrastructure.Extensions
{
    internal static class StringExtensions
    {       
        public static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable()
                .Select(s => s.Trim())
                .ToList();
        }
    }
}
