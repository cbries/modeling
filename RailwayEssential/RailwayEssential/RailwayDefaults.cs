using System;

namespace RailwayEssential
{
    public static class RailwayDefaults
    {
        public const string ProviderName = "RailwayEssentialProject";

        public const string GuidRailwayProjectPkgString = "6f9e0b7c-3adf-4ed5-bfbb-796e2cb3db87";
        public const string GuidRailwayProjectCmdSetString = "892B5DB3-8BB0-4D80-B1C0-AF09A1611F0B";
        public const string GuidRailwayProjectFactoryString = "2221D15B-8AB2-4EDD-B41E-BC422214903F";

        public static readonly Guid GuidRailwayProjectCmdSet = new Guid(GuidRailwayProjectCmdSetString);
        public static readonly Guid GuidRailwayProjectFactory = new Guid(GuidRailwayProjectFactoryString);
    }
}
