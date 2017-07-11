using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecos2Core
{
    public abstract class Command : ICommand
    {
        public string LastError { get; private set; }

        public bool HasError => !string.IsNullOrEmpty(LastError);

        public abstract CommandT Type { get; }
        public abstract string Name { get; }
        public string NativeCommand { get; set; }

        public int ObjectId
        {
            get
            {
                if (Arguments.Count < 1)
                    return -1;

                int vid;
                if (int.TryParse(Arguments[0].Name, out vid))
                    return vid;

                return -1;
            }
        }

        public List<CommandArgument> Arguments { get; set; } = new List<CommandArgument>();

        public virtual bool Parse()
        {
            LastError = null;

            if (string.IsNullOrEmpty(NativeCommand))
            {
                LastError = "Command is empty";
                return false;
            }

            int nOpen = NativeCommand.Count(f => f == '(');
            int nClose = NativeCommand.Count(f => f == ')');
            if (nOpen < 1 || nClose < 1)
            {
                LastError = "Open or closing bracket is missing";
                return false;
            }

            var nativeArguments =
                NativeCommand.Substring(NativeCommand.IndexOf("(", StringComparison.OrdinalIgnoreCase) + 1);

            nativeArguments = nativeArguments.Trim();
            nativeArguments = nativeArguments.TrimEnd(')');
            nativeArguments = nativeArguments.Trim();

            var argumentParts = nativeArguments.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var p in argumentParts)
            {
                if (string.IsNullOrEmpty(p))
                    continue;

                CommandArgument arg = new CommandArgument();
                if (!arg.Parse(p))
                {
                    LastError = "Parsing of argument list failed: " + p;
                    return false;
                }

                Arguments.Add(arg);
            }

            return true;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
                return "";

            if (Arguments.Count <= 0)
                return Name + "()";

            return Name + "(" + string.Join(", ", Arguments) + ")";
        }
    }
}
