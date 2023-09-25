using Microsoft.Playwright;
using NUnit.Framework;
using NUnit_E2E.Settings;
using System.Reflection;
using Allure.Net.Commons;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit_E2E.Helpers;

namespace NUnit_E2E;

/// <summary>
/// The base test class for all tests
/// </summary>
[AllureNUnit]
[AllureParentSuite(AllureSuite)]
public class PageTest
{
    /// <summary>
    /// <inheritdoc cref="System.Reflection.Assembly"/>
    /// <br/>
    /// <inheritdoc cref="Assembly.GetExecutingAssembly"/>
    /// </summary>
    /// <inheritdoc cref="System.Reflection.Assembly"/>
    /// <inheritdoc cref="Assembly.GetExecutingAssembly"/>
    internal static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    /// <summary>
    /// The name of the root suite
    /// </summary>
    internal const string AllureSuite = "Root Suite";
    /// <summary>
    /// Constants for Page object
    /// </summary>
    internal static class PageConsts
    {
        /// <summary>
        /// Constants for reflection of Page.Video object
        /// </summary>
        internal static class VideoReflection
        {
            /// <summary>
            /// The name of the _artifactTcs field in Page.Video._artifactTcs
            /// </summary>
            internal const string _artifactTcs = nameof(_artifactTcs);
            /// <summary>
            /// The name of the Task property Page.Video._artifactTcs.Task
            /// </summary>
            internal const string Task = nameof(Task);
            /// <summary>
            /// The name of the Result property Page.Video._artifactTcs.Task.Result
            /// </summary>
            internal const string Result = nameof(Result);
            /// <summary>
            /// The name of the AbsolutePath property Page.Video._artifactTcs.Task.Result.AbsolutePath
            /// </summary>
            internal const string AbsolutePath = nameof(AbsolutePath);
            /// <summary>
            /// The error message template for when the video absolute path cannot be fetched
            /// </summary>
            internal const string ErrorMessageTemplate = "Error fetching video absolute path: {0}";
        }
        /// <summary>
        /// Constants for Page object
        /// </summary>
        internal static class Browser
        {
            /// <summary>
            /// The directory name where the files from the test will be saved
            /// </summary>
            internal const string AssetsDir = "assets";
            /// <summary>
            /// The video type
            /// </summary>
            internal const string VideoType = "video/webm";
            /// <summary>
            /// The template for the trace file name
            /// </summary>
            internal const string TraceFileTypeTemplate = "{0}{1}{2}.zip";
            /// <summary>
            /// The error message template for when the page cannot be closed
            /// </summary>
            internal const string WaitForLoadStateErrorMessageTemplate = "Error waiting for load state: {0}";
            /// <summary>
            /// The page wait for load state options
            /// <br/>
            /// <inheritdoc cref="PageWaitForLoadStateOptions.Timeout"/>
            /// </summary>
            /// <inheritdoc cref="PageWaitForLoadStateOptions.Timeout"/>
            internal static readonly PageWaitForLoadStateOptions TeardownPageWaitForLoadStateOptions = new PageWaitForLoadStateOptions() { 
                Timeout = 5000
            };
            /// <summary>
            /// <inheritdoc cref="Assembly.Location"/>
            /// <br/>
            /// <inheritdoc cref="Path.GetDirectoryName(string)"/>
            /// </summary>
            /// <inheritdoc cref="Assembly.Location"/>
            /// <inheritdoc cref="Path.GetDirectoryName(string)"/>
            internal static readonly string? GetExecutingAssemblyDirectoryName = Path.GetDirectoryName(Assembly.Location);
            /// <summary>
            /// The user agent to use for the browser
            /// </summary>
            internal const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36";
            /// <summary>
            /// The tracing type
            /// </summary>
            internal const string TracingType = "application/zip";
            /// <summary>
            /// <inheritdoc cref="BrowserTypeLaunchOptions.Args"/>
            /// </summary>
            /// <inheritdoc cref="BrowserTypeLaunchOptions.Args"/>
            internal static readonly IEnumerable<string>? BrowserTypeLaunchOptionsArgs = new[] {
                // "--start-maximized",           // Maximize the browser window
                WindowHelper.WindowSizeCommand,
                // "--window-size=1920,1080",           // Maximize the browser window
                // "--start-in-incognito",        // Start browser in incognito mode
                // "--force-dark-mode",           // Force dark mode
                // "--enable-features=WebUIDarkMode"  // Enable dark mode feature
            };
            /// <summary>
            /// The browser type launch options
            /// </summary>
            internal static readonly BrowserTypeLaunchOptions BrowserTypeLaunchOptions = new BrowserTypeLaunchOptions()
            {
                Headless = Headless,
                Devtools = DevTools,
                Args = BrowserTypeLaunchOptionsArgs,
                DownloadsPath = Path.Combine(GetExecutingAssemblyDirectoryName ?? throw new ArgumentNullException(nameof(GetExecutingAssemblyDirectoryName)), AssetsDir),
                TracesDir = Path.Combine(GetExecutingAssemblyDirectoryName ?? throw new ArgumentNullException(nameof(GetExecutingAssemblyDirectoryName)), AssetsDir),
                // SlowMo = 100,
                // Timeout = 10000,
            };
            /// <summary>
            /// The record video size
            /// </summary>
            internal static RecordVideoSize RecordVideoSize {
                get
                {
                    var recordVideoSize = new RecordVideoSize()
                    {
                        Width = WindowHelper.CurrentHorizontalResolution,
                        Height = WindowHelper.CurrentVerticalResolution
                    };

                    return recordVideoSize;
                }
            }
            /// <summary>
            /// The browser new context options
            /// </summary>
            internal static readonly BrowserNewContextOptions BrowserNewContextOptions = new BrowserNewContextOptions
            {
                UserAgent = UserAgent,
                RecordVideoDir = AbsoluteAssetsDir,
                RecordVideoSize = RecordVideoSize
            };
            /// <summary>
            /// The tracing start options
            /// </summary>
            internal static readonly TracingStartOptions TracingStartOptions = new TracingStartOptions()
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true,
            };
        }
    }
    /// <summary>
    /// <inheritdoc cref="IPlaywright"/>
    /// </summary>
    internal IPlaywright Playwright { get; set; } = null!;
    /// <summary>
    /// <inheritdoc cref="IBrowser"/>
    /// </summary>
    internal IBrowser Browser { get; set; } = null!;
    /// <summary>
    /// <inheritdoc cref="IBrowserContext"/>
    /// </summary>
    internal IBrowserContext Context { get; set; } = null!;
    /// <summary>
    /// <inheritdoc cref="IPage"/>
    /// </summary>
    internal IPage Page { get; set; } = null!;
    /// <summary>
    /// The test app settings object
    /// </summary>
    internal TestAppSettings configuration = null!;
    /// <summary>
    /// <inheritdoc cref="BrowserTypeLaunchOptions.Headless"/>
    /// </summary>
    internal static bool Headless => !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(nameof(Headless).ToUpper()));
    /// <summary>
    /// <inheritdoc cref="BrowserTypeLaunchOptions.Devtools"/>
    /// </summary>
    internal static bool DevTools => string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(nameof(DevTools).ToUpper()));
    /// <summary>
    /// <inheritdoc cref="Assembly.Location"/>
    /// <br/>
    /// <inheritdoc cref="PageConsts.Browser.AssetsDir"/>
    /// </summary>
    /// <inheritdoc cref="PageConsts.Browser.AssetsDir"/>
    /// <inheritdoc cref="Assembly.Location"/>
    /// <inheritdoc cref="Path.Combine"/>
    /// <inheritdoc cref="Path.GetDirectoryName"/>
    internal static string AbsoluteAssetsDir
    {
        get => Path.Combine(Path.GetDirectoryName(Assembly.Location) ?? string.Empty, PageConsts.Browser.AssetsDir);
    }
    /// <summary>
    /// Setup the test
    /// </summary>
    /// <returns></returns>
    [SetUp]
    public async Task BaseSetup()
    {
        WindowHelper.GetScreenDimensionsAndPosition();
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(PageConsts.Browser.BrowserTypeLaunchOptions);
        Context = await Browser.NewContextAsync(PageConsts.Browser.BrowserNewContextOptions);
        await Context.Tracing.StartAsync(PageConsts.Browser.TracingStartOptions);
        Page = await Context.NewPageAsync();
        await Page.SetViewportSizeAsync(WindowHelper.CurrentHorizontalResolution, WindowHelper.CurrentVerticalResolution);
    }
    /// <summary>
    /// Tear down the test
    /// </summary>
    /// <returns></returns>
    [TearDown]
    public async Task BaseTearDown()
    {
        var videoPath = _getVideoAbsolutePath();

        try
        {
            if (!Page.IsClosed) await Page.WaitForLoadStateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format(PageConsts.Browser.WaitForLoadStateErrorMessageTemplate, ex.Message));
        }

        // Get filename without extension
        var traceFileName = string.IsNullOrWhiteSpace(videoPath) ? Guid.NewGuid().ToString("N") : Path.GetFileNameWithoutExtension(videoPath);
        var absoluteTraceFilePath = string.Format(PageConsts.Browser.TraceFileTypeTemplate,
                                                    AbsoluteAssetsDir,
                                                    Path.DirectorySeparatorChar,
                                                    traceFileName);
        // Stop tracing and export it into a zip archive.
        await Context.Tracing.StopAsync(new TracingStopOptions()
        {
            Path = absoluteTraceFilePath
        });

        // If we close the context before we get the video name it will be null and not tied to allure report test
        await Context.CloseAsync();
        await Browser.CloseAsync();

        // Playwright should give us the name and even absolute path of the video file.
        // I will suggest it or even see if I can implement it myself.
        // We could call it before or after close so it would be a cleaner solution.
        // var videoPath = Page.Video.Path;

        // Add the video to the allure report, must be done after the context is closed, otherwise the video file will be locked.
        if (videoPath != null) AllureLifecycle.Instance.AddAttachment(TestContext.CurrentContext.Test.MethodName ?? string.Empty, PageConsts.Browser.VideoType, videoPath);
        // Add the tracing to the allure report
        AllureLifecycle.Instance.AddAttachment(TestContext.CurrentContext.Test.MethodName ?? string.Empty, PageConsts.Browser.TracingType, absoluteTraceFilePath);
    }
    /// <summary>
    /// Get video file path with name from the page object before we close the context, otherwise the video file name won't be in the Page.Video object
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private string? _getVideoAbsolutePath()
    {
        // Ensure we have a page object
        if (Page == null) throw new ArgumentNullException(nameof(Page));

        try
        {
            // Get the non-public _artifactTcs field from the Video object
            var _artifactTcsField = Page?.Video?.GetType().GetField(PageConsts.VideoReflection._artifactTcs, BindingFlags.NonPublic | BindingFlags.Instance);
            var _artifactTcsValue = _artifactTcsField?.GetValue(Page?.Video);

            // Get the Task property and its value
            var taskProperty = _artifactTcsValue?.GetType().GetProperty(PageConsts.VideoReflection.Task);
            var taskValue = taskProperty?.GetValue(_artifactTcsValue) as Task;

            // Get the Result property and its value
            var taskResultProperty = taskValue?.GetType().GetProperty(PageConsts.VideoReflection.Result);
            var artifactResult = taskResultProperty?.GetValue(taskValue);

            // Get the AbsolutePath property and its value
            var absolutePathProperty = artifactResult?.GetType().GetProperty(PageConsts.VideoReflection.AbsolutePath, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var absolutePathValue = absolutePathProperty?.GetValue(artifactResult) as string;

            return absolutePathValue;
        }
        catch (Exception ex)
        {
            // Handle or log the exception as appropriate
            Console.WriteLine(string.Format(PageConsts.VideoReflection.ErrorMessageTemplate, ex.Message));
            return null;  // or rethrow, based on your requirements
        }
    }
    /// <summary>
    /// Tear down the test
    /// </summary>
    [OneTimeTearDown]
    public void BaseOneTimeTearDown()
    {
        Playwright.Dispose();
    }
}