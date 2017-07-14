using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualBasic.FileIO;

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
                using (TextFieldParser parser = new TextFieldParser(_trackFilepath))
                {
                    int lineNumber = 0;

                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(";");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();

                        if (fields == null || fields.Length <= 0)
                            continue;

                        if (lineNumber == 0)
                        {
                            foreach (var name in fields)
                                Heads.Add(name);
                            ++lineNumber;
                            continue;
                        }

                        var info = new TrackInfo();
                        info.Parse(fields);
                        Track.Add(info);

                        ++lineNumber;
                    }
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
