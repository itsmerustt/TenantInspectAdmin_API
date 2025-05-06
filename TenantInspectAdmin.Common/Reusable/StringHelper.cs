using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TenantInspectAdmin.Common.Reusable
{
    public static class StringHelper
    {
        public static string RemoveNonNumeric(string value)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(value, "");
        }

        public static decimal RemoveNonNumericConvertToDecimal(string value)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            return Convert.ToDecimal(digitsOnly.Replace(value, ""));
        }

        public static string ConvertDateTimeToTimeStamp(string dateInput)
        {
            var date = Convert.ToDateTime(dateInput).ToUniversalTime();
            var dateOutput = new DateTimeOffset(date).ToUnixTimeSeconds();

            return dateOutput.ToString();
        }

        public static string GetPropertyName<T>(Expression<Func<T>> propertyLambda)
        {
            var me = propertyLambda.Body as MemberExpression;

            if (me == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }

            return me.Member.Name;
        }

        public static decimal ConvertMeasurementToCmToDecimal(this string value)
        {
            decimal parseValue = 0M;
            try
            {
                if (value == null)
                    return parseValue;

                Regex rgx = new Regex("[^A-Za-z0-9]");
                value = value.Replace("Approximately", "").Replace(" ", "");

                bool containsSpecialCharacter = rgx.IsMatch(value);
                if (containsSpecialCharacter)
                {
                    return parseValue;
                }

                if (value.Contains("cm"))
                {
                    parseValue = RemoveNonNumericConvertToDecimal(value);
                    return parseValue;
                }
                else if (value.Contains("mm"))
                {
                    decimal val = RemoveNonNumericConvertToDecimal(value);
                    parseValue = val > 0 ? val / 10 : val;
                    return parseValue;
                }
                else if (value.Contains("m"))
                {
                    decimal val = RemoveNonNumericConvertToDecimal(value);
                    parseValue = val > 0 ? val * 100 : val;
                    return parseValue;
                }
                else if (value.Contains("inch"))
                {
                    decimal val = RemoveNonNumericConvertToDecimal(value);
                    parseValue = val > 0 ? val * 2.54M : val;
                    return parseValue;
                }
                return parseValue;

            }
            catch (Exception)
            {
                return parseValue;
            }
        }

        public static string GetRegionInfo(string countryEnglishName)
        {
            try
            {
                var regionInfos = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
               .Select(c => new RegionInfo(c.LCID))
               .Distinct()
               .ToList();
                RegionInfo r = regionInfos.Find(
                       region => region.EnglishName.ToLower().Equals(countryEnglishName.Trim().ToLower()));
                return r == null ? "" : r.TwoLetterISORegionName;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetUrlByDate(string url, string minDateCreated, string maxDateCreated, string minDateModified, string maxDateModified)
        {

            //created
            if (minDateCreated != null)
            {
                url += $"min_date_created={minDateCreated}&";
            }
            if (maxDateCreated != null)
            {
                url += $"max_date_created={maxDateCreated}&";
            }

            //modified
            if (minDateModified != null)
            {
                url += $"min_date_modified={minDateModified}&";
            }
            if (maxDateModified != null)
            {
                url += $"max_date_modified={maxDateModified}&";
            }

            return url;
        }


        public static string[] StringToArray(string text, int charCount)
        {
            if (text.Length == 0)
                return Array.Empty<string>();
            var arrayLength = (text.Length - 1) / charCount + 1;
            var result = new String[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                var tmp = "";
                for (var n = 0; n < charCount; n++)
                {
                    var index = i * charCount + n;
                    if (index >= text.Length)   //important if last "part" is smaller
                        break;
                    tmp += text[index];
                }
                result[i] = tmp;
            }
            return result;
        }

    }
}
