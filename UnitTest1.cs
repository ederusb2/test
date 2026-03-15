using System.Text;
using System.Text.Json;
using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using FluentAssertions;

namespace TestProject;

[AllureNUnit]
[AllureSuite("Test Suite")]
public class Tests
{

    [SetUp]
    public void SetEnvironment()
    {
        var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "allure-results", "environment.properties");

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        File.WriteAllText(path,
            @"Browser=Chrome
            Browser.Version=123
            Environment=QA
            OS=Windows 11
            URL=https://www.a1qa.com");
    }

    [SetUp]
    public void SetExecutors()
    {
        var executor = new
        {
            name = "Jenkins",
            type = "jenkins",
            url = "http://localhost:6969",
            buildName = "System Tests",
            buildUrl = "http://http://localhost:6969/job/ui-tests/125/"
        };

        var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "allure-results", "executor.json");

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        File.WriteAllText(path, JsonSerializer.Serialize(executor));
    }

    [SetUp]
    public void CreateCategories()
    {
        var json = File.ReadAllText(Path.Combine("..", "..", "..", "categories.json"));

        var path = Path.Combine(
            TestContext.CurrentContext.WorkDirectory,
            "allure-results",
            "categories.json");

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, json);
    }

    [SetUp]
    [AllureBefore("Initial setup")]
    public void Setup()
    {
        Console.WriteLine("Initial setup");
    }

    [Test]
    [AllureTag("UI")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureFeature("Feature №1")]
    [AllureStep("Check message type")]
    [AllureLink("TC-1")]
    [AllureIssue("Bug-123")]
    public void Test1()
    {
        const string message = "Hello, World!";
        message.Should().BeOfType<string>("Message has not correct type");
    }

    [TearDown]
    [AllureAfter("After step")]
    public void After()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;

        if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            AllureApi.AddAttachment("Failure info", "text/plain", Encoding.UTF8.GetBytes("Test failed"));
        }
        else
        {
            AllureApi.AddAttachment("Success info", "text/plain", Encoding.UTF8.GetBytes("Test passed"));
        }
    }
}
