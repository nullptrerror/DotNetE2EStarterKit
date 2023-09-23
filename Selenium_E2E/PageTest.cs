using MicrosoftPlaywrightPlaywright = Microsoft.Playwright.Playwright;
using Microsoft.Playwright;
using NUnit.Framework;
using NUnit_E2E.Settings;
using System.Reflection;

namespace NUnit_E2E;

public class PageTest
{
    internal const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36";
    internal bool Headless => !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(nameof(Headless).ToUpper()));
    internal IPlaywright Playwright { get; set; } = null!;
    internal IBrowser Browser { get; set; } = null!;
    internal IBrowserContext Context { get; set; } = null!;
    internal IPage Page { get; set; } = null!;
    internal TestAppSettings configuration = null!;
    internal string currentVideoDir
    {
        get
        {
            // Get the current executing directory
            string executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new Exception("Could not get executing directory");

            // Build the video directory path relative to the executing directory using the appropriate directory separator
            string videoDirectory = Path.Combine(executingDirectory, $"videos{Path.DirectorySeparatorChar}{TestContext.CurrentContext.Test.MethodName}{Path.DirectorySeparatorChar}");

            return videoDirectory;
        }
    }


    [SetUp]
    public async Task BaseSetup()
    {
        Playwright = await MicrosoftPlaywrightPlaywright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = Headless });
        Context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            UserAgent = UserAgent,
            RecordVideoDir = currentVideoDir,
            RecordVideoSize = new RecordVideoSize { Width = 1280, Height = 720 }
        });
        Page = await Context.NewPageAsync();
    }

    [TearDown]
    public async Task BaseTearDown()
    {
        await Context.CloseAsync();
        await Browser.CloseAsync();
        // Rename the video after the context is closed
        var videoFiles = Directory.GetFiles(currentVideoDir);
        if (videoFiles.Length > 0)
        {
            var videoFilePath = videoFiles[0];
            var parentDir = Directory.GetParent(videoFilePath)?.Parent?.FullName ?? throw new Exception("Could not get parent directory");
            var desiredVideoName = Path.Combine(parentDir, $"{DateTime.Now:yyyyMMdd_HHmmss}_{TestContext.CurrentContext.Test.MethodName}.webm");
            File.Move(videoFilePath, desiredVideoName);
            // Delete directory if it exists
            if (Directory.Exists(currentVideoDir))
            {
                Directory.Delete(currentVideoDir, true);
            }
        }
    }

    [OneTimeTearDown]
    public void BaseOneTimeTearDown()
    {
        Playwright.Dispose();
    }
}