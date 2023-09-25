using NUnit.Allure.Attributes;
using NUnit.Framework;
using System.Collections;

namespace NUnit_E2E.TestPlan
{
    public partial class Scenario1 : PageTest
    {
        internal static IEnumerable TestCasesSearches
        {
            get
            {
                yield return new TestCaseData("Texas").Returns("texas - Google Search");
                yield return new TestCaseData("Florida").Returns("florida - Google Search");
                yield return new TestCaseData("California").Returns("california - Google Search");
            }
        }

        [Test]
        [AllureSubSuite(nameof(Scenario1_GoogleSearch))]
        [AllureTag(nameof(Scenario1_GoogleSearch))]
        [TestCaseSource(typeof(Scenario1), nameof(TestCasesSearches))]
        public async Task<string> Scenario1_GoogleSearch(string q)
        {
            await Page.GotoAsync("https://www.google.com/");

            await Page.GetByLabel("Search", new() { Exact = true }).ClickAsync();

            await Page.GetByLabel("Search", new() { Exact = true }).FillAsync(q);

            await Page.GetByText(q, new() { Exact = true }).ClickAsync();

            return await Page.TitleAsync();
        }
    }
}