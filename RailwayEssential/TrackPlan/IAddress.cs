using Communicator;

namespace TrackPlan
{
    public interface IAddress
    {
        CommandStation Station { get; set; }
        int Index { get; set; }
    }
}
