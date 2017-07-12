using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RailwayEssentialWeb;

namespace RailwayEssentialWebTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var targetPath = @"C:\temp\";

            var gen = new WebTableGenerator()
            {
                //ThemeDirectory = @"C:\Users\ChristianRi\Desktop\Github\modeling\RailwayEssential\RailwayEssentialUi\Resources\Theme\SpDrS60"
                ThemeDirectory = @"C:\Users\cries\Source\Repos\modeling\RailwayEssential\RailwayEssentialUi\Resources\Theme\SpDrS60"
            };
            gen.Generate(targetPath);
        }
    }
}
