using System.Collections.Generic;

namespace TrackPlan
{
    public class Routes : IRoutes
    {
        private static int _instanceCounter = 0;

        private readonly List<IAccessory> _accessories = new List<IAccessory>();
        
        public string Identifier { get; set; }

        public IReadOnlyList<IAccessory> Accessories => _accessories;

        public Routes()
        {
            Identifier = "Routes " + _instanceCounter;
            ++_instanceCounter;
        }

        public void Execute()
        {
            
        }
    }
}
