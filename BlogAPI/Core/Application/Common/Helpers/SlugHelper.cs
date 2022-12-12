using BlogAPI.Core.Application.Common.Interfaces;
using System.Globalization;
using System.Text;

namespace BlogAPI.Core.Application.Common.Helpers
{
    public class SlugHelper : ISlugHelper
    {
        public string CreateSlug(string title)
        {
            title = title.ToLower().Replace(' ','_');
            var normalizedString = title.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark &&
                    unicodeCategory != UnicodeCategory.OpenPunctuation &&
                    unicodeCategory != UnicodeCategory.ClosePunctuation &&
                    unicodeCategory != UnicodeCategory.ConnectorPunctuation &&
                    unicodeCategory != UnicodeCategory.DashPunctuation &&
                    unicodeCategory != UnicodeCategory.FinalQuotePunctuation &&
                    unicodeCategory != UnicodeCategory.InitialQuotePunctuation &&
                    unicodeCategory != UnicodeCategory.OtherPunctuation)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }
    }
}
