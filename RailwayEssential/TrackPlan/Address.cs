using Communicator;

namespace TrackPlan
{
    public class Address : IAddress
    {
        public CommandStation Station { get; set; }
        public int Index { get; set; }

        public Address()
        {
            Index = -1;
        }
    }
}
