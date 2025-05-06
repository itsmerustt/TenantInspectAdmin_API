using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenantInspectAdmin.Common.Reusable
{
    public static class KeywordsHandler
    {
        public static IList<string> GetMetaKeywords(string metaKeywordsString)
        {
            IList<string> metaKeywords = new List<string>();

            if (metaKeywordsString != null)
            {
                var arrayKeywords = metaKeywordsString.Split(',');

                if (arrayKeywords != null && arrayKeywords.Count() > 0)
                {
                    foreach (var keyword in arrayKeywords)
                    {
                        if (!String.IsNullOrEmpty(keyword.Trim()))
                        {
                            metaKeywords.Add(keyword.Trim());
                        }
                    }
                }
            }
            return metaKeywords;
        }

        public static string GetSearchKeyWords(string[] tagsArray)
        {
            var searchKeyWords = "";


            if (tagsArray != null && tagsArray.Count() > 0)
            {
                foreach (var tag in tagsArray)
                {
                    if (!String.IsNullOrEmpty(tag.Trim()))
                    {
                        searchKeyWords = searchKeyWords + "," + tag.Trim();
                    }
                }
            }
            return searchKeyWords.Trim(',');
        }
    }
}
