using Newtonsoft.Json.Linq;

namespace Theme
{
    public class ThemeItemRoute
    {
        public string Value { get; set; }

        public bool Parse(JToken tkn)
        {
            var v = tkn.ToString();
            if (string.IsNullOrEmpty(v))
                return false;

            Value = v;

            return true;
        }
    }
}
