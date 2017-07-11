using Ecos2Core;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ecos2CoreTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var cmd0 = CommandFactory.Create("request(1002, view)");
            cmd0.Name.ShouldBeEquivalentTo("request");
            cmd0.Arguments.Count.Should().Be(2);

            var cmd1 = CommandFactory.Create("get(100)");
            cmd1.Name.ShouldBeEquivalentTo("get");
            cmd1.Arguments.Count.Should().Be(1);

            var cmd2 = CommandFactory.Create("release( 11, view, viewswitch)");
            cmd2.Name.ShouldBeEquivalentTo("release");
            cmd2.Arguments.Count.Should().Be(3);

            var cmd3 = CommandFactory.Create("set( 1, stop)");
            cmd3.Name.ShouldBeEquivalentTo("set");
            cmd3.Arguments.Count.Should().Be(2);

            var cmd4 = CommandFactory.Create("get( 26, state)");
            cmd4.Name.ShouldBeEquivalentTo("get");
            cmd4.Arguments.Count.Should().Be(2);

            var cmd5 = CommandFactory.Create("release(10, view)");
            cmd5.Name.ShouldBeEquivalentTo("release");
            cmd5.Arguments.Count.Should().Be(2);

            var cmd6 = CommandFactory.Create("create(10,  addr[1000], name[\"Ae3/6II SBB\"],   protocol[DCC14], append)");
            cmd6.Name.ShouldBeEquivalentTo("create");
            cmd6.Arguments.Count.Should().Be(5);
            cmd6.ToString().ShouldBeEquivalentTo("create(10, addr[1000], name[\"Ae3/6II SBB\"], protocol[DCC14], append)");

            var cmd7 = CommandFactory.Create("ries()");
            cmd7.Name.ShouldBeEquivalentTo("Unknown");
            cmd7.Arguments.Count.Should().Be(0);

            var cmd8 = CommandFactory.Create("test()");
            cmd8.Name.ShouldBeEquivalentTo("test");
            cmd8.Arguments.Count.Should().Be(0);

            var cmd9 = CommandFactory.Create("test(\"Hello world!\")");
            cmd9.Name.ShouldBeEquivalentTo("test");
            cmd9.Arguments.Count.Should().Be(1);
        }

        [TestMethod]
        public void CheckArgument0()
        {
            CommandArgument arg0 = new CommandArgument {Name = "addr"};
            arg0.Parameter.Add("1000");
            arg0.ToString().Should().BeEquivalentTo("addr[1000]");

            CommandArgument arg1 = new CommandArgument { Name = "name" };
            arg1.Parameter.Add("Ae3/6II SBB");
            arg1.ToString().Should().BeEquivalentTo("name[\"Ae3/6II SBB\"]");

            CommandArgument arg2 = new CommandArgument {Name = "addr"};
            arg2.Parameter.Add("1000");
            arg2.Parameter.Add("1010");
            arg2.Parameter.Add("1020");
            arg2.ToString().Should().BeEquivalentTo("addr[1000,1010,1020]");
        }
    }
}
