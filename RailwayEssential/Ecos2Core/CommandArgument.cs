using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecos2Core
{
    public class CommandArgument
    {
        private List<string> _parameter = new List<string>();

        public string Name { get; set; }

        public List<string> Parameter
        {
            get { return _parameter; }
            set { _parameter = value; }
        }

        public override string ToString()
        {
            if (Parameter.Count <= 0)
                return Name;

            for(int i=0; i < Parameter.Count; ++i)
            {
                var p = Parameter[i];
                if (p.IndexOf(" ", StringComparison.OrdinalIgnoreCase) != -1)
                    Parameter[i] = "\"" + p + "\"";
            }

            return string.Format("{0}[{1}]", Name, string.Join(",", Parameter));
        }

        public bool Parse(string argument, bool keepQuotes=false)
        {
            if (string.IsNullOrEmpty(argument))
                return false;

            if(argument.IndexOf("[", StringComparison.OrdinalIgnoreCase) != -1 && argument.IndexOf("]", StringComparison.OrdinalIgnoreCase) != -1)
            {
                int index = argument.IndexOf("[", StringComparison.OrdinalIgnoreCase);

                Name = argument.Substring(0, index).Trim();
                var args = argument.Substring(index + 1).Trim().TrimEnd(']').Trim();
                if (string.IsNullOrEmpty(args))
                    Parameter.Clear();
                else
                {
                    Parameter = args.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();
                    for (int i = 0; i < Parameter.Count; ++i)
                    {
                        if (string.IsNullOrEmpty(Parameter[i]))
                            continue;

                        if(!keepQuotes)
                            Parameter[i] = Parameter[i].Trim().Trim('"');
                    }
                }
            }
            else
            {
                Name = argument.Trim();
                Parameter.Clear();
            }

            return true;
        }
    }
}
