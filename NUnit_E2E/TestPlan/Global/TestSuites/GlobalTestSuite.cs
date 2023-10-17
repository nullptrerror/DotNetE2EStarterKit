using NUnit.Framework;
using NUnit_E2E.TestFixture.PlaywrightTest;
using Serilog;

namespace NUnit_E2E.TestPlan.Httpbin.TestSuites;

[TestFixture]
[Parallelizable(ParallelScope.Children)]
public sealed class GlobalTestSuite : BrowserTest
{
    [TestCase("https://www.google.com/")]
    [TestCase("https://www.facebook.com/")]
    [TestCase("https://www.youtube.com/")]
    [TestCase("https://www.amazon.com/")]
    [TestCase("https://www.wikipedia.org/")]
    [TestCase("https://twitter.com/")]
    public async Task GotoAsync(string url)
    {
        Log.Information(nameof(GotoAsync) + " (start) " + TestContext.CurrentContext.Test.ID);
        await Page.GotoAsync(url);
        Assert.That(Page.Url, Does.StartWith(url));
        Log.Information(nameof(GotoAsync) + " (end) " + TestContext.CurrentContext.Test.ID);
    }
}
