using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json.Linq;

namespace TrackPlanParser
{
    public class TrackPlanParser
    {
        private readonly string _trackFilepath;

        public List<string> Heads { get; set; }

        public Track Track { get; private set; }

        public TrackPlanParser(string filepath)
        {
            _trackFilepath = filepath;
            Heads = new List<string>();
            Track = new Track();
        }

        public bool Parse()
        {
            if (!File.Exists(_trackFilepath))
                return false;

            try
            {
                string cnt = File.ReadAllText(_trackFilepath);
                if (string.IsNullOrEmpty(cnt))
                    return false;

                JArray ar = JArray.Parse(cnt);
                if (ar == null || ar.Count <= 0)
                    return true;

                foreach (var o in ar)
                {
                    JObject oo = o as JObject;
                    if (oo == null)
                        continue;

                    var info = new TrackInfo();
                    info.Parse(oo);
                    Track.Add(info);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("<Parser> " + ex.Message);
                return false;
            }

            return true;
        }
    }
}
