using Microsoft.Playwright;
using NUnit.Allure.Attributes;
using NUnit.Framework;
using System;
using System.Collections;

namespace NUnit_E2E.TestSuitScenarios
{
    public partial class Goto : PageTest
    {
        internal static IEnumerable TestCasesVisits
        {
            get
            {
                yield return new TestCaseData("https://www.google.com/").Returns("Google");
                yield return new TestCaseData("https://www.chase.com/").Returns("Credit Card, Mortgage, Banking, Auto | Chase Online | Chase.com");
                yield return new TestCaseData("https://www.defense.gov/").Returns("U.S. Department of Defense");
            }
        }

        [Test]
        [AllureSubSuite(nameof(GotoAsync))]
        [AllureTag(nameof(GotoAsync))]
        [TestCaseSource(typeof(Goto), nameof(TestCasesVisits))]
        public async Task<string> GotoAsync(string url)
        {
            // navigate to url
            await Page.GotoAsync(url);

            //  verify theres no other loading activity
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await Page.WaitForLoadStateAsync(LoadState.Load);

            // return page title for verification
            return await Page.TitleAsync();
        }
    }
}