using System.Collections.Generic;

namespace TrackPlan
{
    public interface IRoutes
    {
        string Identifier { get; set; }
        IReadOnlyList<IAccessory> Accessories { get; }

        void Execute();
    }
}
