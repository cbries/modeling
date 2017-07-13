using System;
using System.Collections.Generic;
using Ecos2Core;
using Newtonsoft.Json.Linq;

namespace TrackInformation
{
    public class S88 : Item
    {
        #region Properties

        private int _index;

        /// <summary> the position within the S88 bus </summary>
        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                OnPropertyChanged();
                OnPropertyChanged("Title");
            }
        }

        private int _ports;

        public int Ports
        {
            get => _ports;
            set
            {
                _ports = value;
                OnPropertyChanged();
                OnPropertyChanged("Title");
            }
        }

        private string _stateOriginal;

        public string StateOriginal
        {
            get => _stateOriginal;
            set
            {
                _stateOriginal = value;
                OnPropertyChanged();
                OnPropertyChanged("Title");
            }
        }

        public string StateBinary => ToBinary(StateOriginal);

        private string ToBinary(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                string m = "";
                for (int i = 0; i < _ports; ++i)
                    m += "0";
                return m;
            }

            return Convert.ToString(Convert.ToInt64(hex, 16), 2).PadLeft(16, '0');
        }

        public override void UpdateTitle()
        {
            Title = $"{ObjectId} {Index}:{Ports} {StateBinary}";
            OnPropertyChanged("Title");
        }

        #endregion

        public override void Parse(List<CommandArgument> arguments)
        {
            foreach (var arg in arguments)
            {
                if (arg == null)
                    continue;

                if (arg.Name.Equals("ports", StringComparison.OrdinalIgnoreCase))
                {
                    int v;
                    if (int.TryParse(arg.Parameter[0], out v))
                        Ports = v;
                    else
                        Ports = -1;
                }
            }
        }

        public override JObject ToJson()
        {
            JObject o = new JObject
            {
                ["index"] = Index,
                ["ports"] = Ports,
                ["stateOriginal"] = StateOriginal
            };
            return o;
        }

        public override void ParseJson(JObject o)
        {
            if (o["index"] != null)
                Index = (int) o["index"];
            if (o["ports"] != null)
                Ports = (int) o["ports"];
            if (o["stateOriginal"] != null)
                StateOriginal = o["stateOriginal"].ToString();
        }
    }
}
