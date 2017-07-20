using System;

namespace RailwayEssentialMdi.Interfaces
{
    public interface IContent
    {
        string Title { get; }

        event EventHandler Closing;
    }
}
