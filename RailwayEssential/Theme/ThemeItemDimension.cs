using Newtonsoft.Json.Linq;

namespace Theme
{
    public class ThemeItemDimension
    {
        public string Value { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public bool Parse(JToken tkn)
        {
            var v = tkn.ToString();
            if (string.IsNullOrEmpty(v))
                return false;

            Value = v;

            var parts = Value.Split(new[] {'x'});

            X = 1;
            Y = 1;

            if (parts.Length == 2)
            {
                int vv;
                if (int.TryParse(parts[0], out vv))
                    X = vv;
                if (int.TryParse(parts[1], out vv))
                    Y = vv;
            }

            return true;
        }

    }
}
