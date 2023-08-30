using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium_E2E.Helpers;
using Selenium_E2E.Settings;

namespace Selenium_E2E;

public class BaseTestPlan
{
    internal IWebDriver driver = null!;
    internal TestAppSettings configuration = null!;
    internal WebDriverWait wait = null!;


    [SetUp]
    public void BaseSetup()
    {
        // Webdriver helper
        driver = WebDriverHelper.SetupManageAndCreateWebDriver();
        // Maximize window
        driver.Manage().Window.Maximize();
        // Set page load timeout
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        // Set script timeout
        driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(30);
        // Delete cache
        driver.Manage().Cookies.DeleteAllCookies();
        // Set configuration
        configuration = ConfigurationHelper.Build();
        // Standard wait
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
    }

    [TearDown]
    public void BaseTearDown()
    {   
        // Dispose chrome driver
        driver.Dispose();
    }
}