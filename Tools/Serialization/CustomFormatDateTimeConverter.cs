using Newtonsoft.Json.Converters;

namespace ShowsService.Tools.Serialization
{
    public class CustomFormatDateTimeConverter : IsoDateTimeConverter
    {
        public CustomFormatDateTimeConverter(string dateTimeFormat)
        {
            this.DateTimeFormat = dateTimeFormat;
        }
    }
}