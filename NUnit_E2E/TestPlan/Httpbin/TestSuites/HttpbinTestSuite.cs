using NUnit.Framework;
using NUnit_E2E.TestFixture.PlaywrightTest;

namespace NUnit_E2E.TestPlan.Httpbin.TestSuites;

[TestFixture]
[Parallelizable(ParallelScope.Children)]
public sealed class HttpbinTestSuite : BrowserTest
{
    [TestCase("get/200")]
    [TestCase("basic-auth/username/password")]
    [TestCase("headers")]
    [TestCase("ip")]
    [TestCase("user-agent")]
    [TestCase("cache")]
    [TestCase("cache/60")]
    [TestCase("etag/etag")]
    [TestCase("response-headers")]
    [TestCase("brotli")]
    [TestCase("defalte")]
    [TestCase("deny")]
    [TestCase("encoding/utf8")]
    [TestCase("gzip")]
    [TestCase("html")]
    [TestCase("json")]
    [TestCase("robots.txt")]
    [TestCase("xml")]
    public async Task GotoAsync(string endpoint)
    {
        string url = "https://httpbin.org/" + endpoint;
        await Page.GotoAsync(url);
        Assert.That(Page.Url, Does.StartWith(url));
    }
}
