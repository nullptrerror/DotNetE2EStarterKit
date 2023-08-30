using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace Selenium_E2E.Helpers
{
    public static class WebDriverHelper
    {
        public static IWebDriver SetupManageAndCreateWebDriver()
        {
            // Setup ChromeDriver using WebDriver Manager and create a new Chrome browser instance
            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            return new ChromeDriver();
        }
    }
}