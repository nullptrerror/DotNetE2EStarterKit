using NUnit.Framework;
using NUnit_E2E;

[SetUpFixture]
public sealed class SetUpFixture
{
    private const string _allureResultsPath = "allure-results";
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        string allureResultsPath = Path.Combine(TestContext.CurrentContext.WorkDirectory,
                                                PageTest.PageConsts.Browser.AssetsDir,
                                                _allureResultsPath);

        if (!Directory.Exists(allureResultsPath)) Directory.CreateDirectory(allureResultsPath);
    }
}