﻿using System;
using System.Collections.Generic;
using Ecos2Core;

namespace TrackInformation
{
    public class Ecos2 : Item
    {
        public string Name => "ECoS2";

        private readonly string[] _fields = new string[4];

        public string ProtocolVersion
        {
            get => _fields[0];
            set
            {
                _fields[0] = value;
                OnPropertyChanged();
            }
        }

        public string ApplicationVersion
        {
            get => _fields[1];
            set
            {
                _fields[1] = value;
                OnPropertyChanged();
            }
        }

        public string HardwareVersion
        {
            get => _fields[2];
            set
            {
                _fields[2] = value;
                OnPropertyChanged();
            }

        }

        public string Status
        {
            get => _fields[3];
            set
            {
                _fields[3] = value;
                OnPropertyChanged();
            }

        }

        public override void Parse(List<CommandArgument> arguments)
        {
            foreach (var arg in arguments)
            {
                if (arg == null)
                    continue;

                if (arg.Name.Equals("status", StringComparison.OrdinalIgnoreCase))
                    Status = arg.Parameter[0];
                else if (arg.Name.Equals("ProtocolVersion", StringComparison.OrdinalIgnoreCase))
                    ProtocolVersion = arg.Parameter[0];
                else if (arg.Name.Equals("ApplicationVersion", StringComparison.OrdinalIgnoreCase))
                    ApplicationVersion = arg.Parameter[0];
                else if (arg.Name.Equals("HardwareVersion", StringComparison.OrdinalIgnoreCase))
                    HardwareVersion = arg.Parameter[0];
            }

        }
    }
}