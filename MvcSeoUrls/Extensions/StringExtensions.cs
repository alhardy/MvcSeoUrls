using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MvcSeoUrls.Extensions
{
    public static class StringExtensions
    {
        public static string EscapeName(this string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                name = NormalizeString(name);

                var builder = new StringBuilder();
                foreach (var t in name)
                {
                    builder.Append(char.IsLetterOrDigit(t) ? t : ' ');
                }

                name = builder.ToString();

                name = Regex.Replace(name, @"[ ]{1,}", @"-", RegexOptions.None);
            }

            return name;
        }
      
        public static string NormalizeString(this string value)
        {
            var normalizedFormD = value.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (var t in from t in normalizedFormD let uc = CharUnicodeInfo.GetUnicodeCategory(t) where uc != UnicodeCategory.NonSpacingMark select t)
            {
                builder.Append(t);
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}