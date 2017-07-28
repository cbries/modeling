﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RailwayEssentialCore;

namespace RailwayEssentialMdi.DataObjects
{
    public class ProjectFile : IPersist
    {
        public string Filepath { get; private set; }
        public string Dirpath { get; private set; }

        public string Name { get; set; }
        public float Version { get; set; }
        public string TargetHost { get; set; }
        public UInt16 TargetPort { get; set; }
        public List<string> Objects { get; set; }
        public ProjectTrack Track { get; set; }

        public ProjectFile()
        {
            Objects = new List<string>();
            Track = new ProjectTrack();
        }

        public bool Load(string path)
        {
            try
            {
                Filepath = path;
                Dirpath = Path.GetDirectoryName(path);

                if (!File.Exists(path))
                    return false;

                string cnt = File.ReadAllText(path, Encoding.UTF8);

                if (string.IsNullOrEmpty(cnt))
                    return false;

                JObject o = JObject.Parse(cnt);

                if (o["name"] != null)
                    Name = o["name"].ToString();

                if (o["version"] != null)
                {
                    float v;
                    Version = float.TryParse(o["version"].ToString(), out v) ? v : 1.0f;
                }

                if (o["targetHost"] != null)
                    TargetHost = o["targetHost"].ToString();

                if (o["targetPort"] != null)
                {
                    UInt16 v;
                    if (UInt16.TryParse(o["targetPort"].ToString(), out v))
                        TargetPort = v;
                    else
                        TargetPort = 15471;
                }

                if (o["objects"] != null)
                {
                    JArray ar = o["objects"] as JArray;
                    if (ar != null)
                    {
                        foreach (var e in ar)
                        {
                            if (e == null)
                                continue;
                            if (string.IsNullOrEmpty(e.ToString()))
                                continue;
                            Objects.Add(e.ToString());
                        }
                    }
                }

                if (o["track"] != null)
                {
                    JObject ao = o["track"] as JObject;
                    if (ao != null)
                    {
                        var item = new ProjectTrack();
                        if (item.Parse(ao))
                            Track = item;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("<Project> " + ex.Message);
                return false;
            }
        }

        private JObject ToJson()
        {
            JArray objects = new JArray();
            foreach (var e in Objects)
                objects.Add(e);

            JObject o = new JObject
            {
                ["name"] = Name,
                ["version"] = Version,
                ["targetHost"] = TargetHost,
                ["targetPort"] = TargetPort,
                ["tracks"] = Track.ToJson(),
                ["objects"] = objects
            };

            return o;
        }

        #region IPersist

        public bool Save()
        {
            return Save(Filepath);
        }

        public bool Save(string targetFilepath)
        {
            var cnt = ToJson().ToString(Formatting.Indented);
            try
            {
                File.WriteAllText(targetFilepath, cnt, Encoding.UTF8);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
