using Microsoft.Playwright;
using NUnit.Allure.Attributes;
using NUnit.Framework;
using System.Collections;

namespace NUnit_E2E.TestPlan
{
    public class Scenario0 : Tests
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
        [AllureSubSuite(nameof(Scenario0_GotoAsync))]
        [AllureTag(nameof(Scenario0_GotoAsync))]
        [TestCaseSource(typeof(Scenario0), nameof(TestCasesVisits))]
        public async Task<string> Scenario0_GotoAsync(string url)
        {
            // navigate to url
            await Page.GotoAsync(url);

            await Page.WaitForLoadStateAsync();

            // return page title for verification
            return await Page.TitleAsync();
        }
    }
}