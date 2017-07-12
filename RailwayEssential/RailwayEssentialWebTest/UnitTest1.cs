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
            var targetPath = @"C:\temp\TestMethod1.html";

            var gen = new WebTableGenerator()
            {
                ThemeDirectory = @"C:\Users\ChristianRi\Desktop\Github\modeling\RailwayEssential\RailwayEssentialUi\Resources\Theme\SpDrS60"
            };
            gen.Generate(targetPath);
        }
    }
}
