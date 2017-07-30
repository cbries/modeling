using System;

namespace RailwayEssentialMdi.Interfaces
{
    public interface IContent
    {
        string Name { get; }

        event EventHandler Closing;
    }
}
