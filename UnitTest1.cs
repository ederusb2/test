using FluentAssertions;

namespace TestProject;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        const string message = "Hello, World!";
        message.Should().BeOfType<string>("Message has not correct type");
    }
}
