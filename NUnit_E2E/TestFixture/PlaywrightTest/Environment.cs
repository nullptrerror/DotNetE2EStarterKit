using Microsoft.Playwright;
using Microsoft.Playwright.TestAdapter;
using NUnit.Framework;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Text;

namespace NUnit_E2E.TestFixture.PlaywrightTest;

internal sealed class Environment
{
    private const string AssetsDirectory = "assets";
    private const string _traceExtension = ".trace.playwright.dev.zip";
    private const string _videoExtension = ".video.playwright.webm";
    private const string _prefix = nameof(Microsoft) + "_" + nameof(Playwright) + "_" + nameof(Environment) + "_";
    private static string _testCommonFileName
    {
        get
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var testName = TestContext.CurrentContext.Test.ID;
            var safeName = new StringBuilder(testName.Length + 36); // 36 for GUID and underscores
            foreach (var c in testName) safeName.Append(invalidChars.Contains(c) ? '_' : c);
            safeName.Append('_');
            safeName.Append(testName);
            safeName.Append('_');
            safeName.Append(Guid.NewGuid().ToString("N"));
            return safeName.ToString();
        }
    }
    internal static readonly PageWaitForLoadStateOptions EndPageWaitForLoadStateOptions = new PageWaitForLoadStateOptions { Timeout = 5000 };
    internal static BrowserNewContextOptions? BrowserNewContextOptions
    {
        get
        {
            // Cache environment variables and other properties
            var width = 1280;
            var height = 720;
            var widthStr = GetEnvVarValueFor<ViewportSize>(o => o.Width);
            var heightStr = GetEnvVarValueFor<ViewportSize>(o => o.Height);
            var username = GetEnvVarValueFor<HttpCredentials>(o => o.Username)?.Trim();
            var password = GetEnvVarValueFor<HttpCredentials>(o => o.Password)?.Trim();
            var recordVideoDirEnv = GetEnvVarValueFor<BrowserNewContextOptions>(o => o.RecordVideoDir);

            bool hasHttpCredentials = !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
            bool hasViewportAndRecordVideoDir =
                int.TryParse(widthStr, out width) &&
                int.TryParse(heightStr, out height) &&
                !string.IsNullOrEmpty(recordVideoDirEnv);

            // Case: HttpCredentials and (ViewportSize and RecordVideoDir) both provided
            if (hasHttpCredentials && hasViewportAndRecordVideoDir)
            {
                return new BrowserNewContextOptions
                {
                    HttpCredentials = new HttpCredentials { Username = username!, Password = password! },
                    ViewportSize = new ViewportSize { Width = width, Height = height },
                    RecordVideoDir = Path.Combine(TestContext.CurrentContext.TestDirectory, recordVideoDirEnv!)
                };
            }
            // Case: Only HttpCredentials provided
            else if (hasHttpCredentials)
            {
                return new BrowserNewContextOptions
                {
                    HttpCredentials = new HttpCredentials { Username = username!, Password = password! }
                };
            }
            // Case: Only (ViewportSize and RecordVideoDir) provided
            else if (hasViewportAndRecordVideoDir)
            {
                return new BrowserNewContextOptions
                {
                    ViewportSize = new ViewportSize { Width = width, Height = height },
                    RecordVideoDir = Path.Combine(TestContext.CurrentContext.TestDirectory, recordVideoDirEnv!)
                };
            }
            // Case: Neither provided
            else
            {
                return null;
            }
        }
    }
    internal static TracingStartOptions? TracingStartOptions
    {
        get => bool.TryParse(GetEnvVarValueFor<TracingStartOptions>(o => o.Screenshots), out bool screenshots) &&
        bool.TryParse(GetEnvVarValueFor<TracingStartOptions>(o => o.Snapshots), out bool snapshots) &&
        bool.TryParse(GetEnvVarValueFor<TracingStartOptions>(o => o.Sources), out bool sources) &&
        (screenshots || snapshots || sources) ? new TracingStartOptions()
        {
            Screenshots = screenshots,
            Snapshots = snapshots,
            Sources = sources,
            Title = TestContext.CurrentContext.Test.ID + _traceExtension
        } : null;
    }
    internal static TracingStopOptions TracingStopOptions
    {
        get => new TracingStopOptions()
        {
            Path = Path.Combine(TestContext.CurrentContext.TestDirectory, GetEnvVarValueFor<TracingStopOptions>(o => o.Path) ?? AssetsDirectory, _testCommonFileName + _traceExtension)
        };
    }
    internal static async Task SetUpAsync(ConcurrentDictionary<string, ScopedTestContext> scopedTestContextStorage, IBrowser browser, string testId)
    {
        var context = await browser.NewContextAsync(BrowserNewContextOptions).ConfigureAwait(false);
        scopedTestContextStorage.TryAdd(testId,
                                        new ScopedTestContext(
                                            context: context,
                                            page: await context.NewPageAsync().ConfigureAwait(false)));
        var tracingStartOptions = TracingStartOptions;
        if (tracingStartOptions?.ShouldStartOrStopTracing() ?? false) await context.Tracing.StartAsync(tracingStartOptions).ConfigureAwait(false);
    }
    internal static async Task TearDownAsync(ConcurrentDictionary<string, ScopedTestContext> scopedTestContextStorage, string testId)
    {
        if (scopedTestContextStorage.TryRemove(testId, out ScopedTestContext? scopedTestContext))
        {
            try
            {
                await scopedTestContext.Page.WaitForLoadStateAsync(LoadState.NetworkIdle, EndPageWaitForLoadStateOptions).ConfigureAwait(false);
            } catch (Exception) { }
            var tracingStartOptions = TracingStartOptions;
            var tracingStopOptions = TracingStopOptions;
            if (tracingStartOptions.ShouldStartOrStopTracing())
            {
                await scopedTestContext.Page.Context.Tracing.StopAsync(tracingStopOptions);
                TestContext.AddTestAttachment(tracingStopOptions.Path!, "Playwright Trace");
            }
            await scopedTestContext.Page.CloseAsync();
            await scopedTestContext.Context.CloseAsync();
            if (BrowserNewContextOptions != null)
            {
                var videoPath = await scopedTestContext.Page.Video!.PathAsync();
                // Rename the video file to match the test name
                var newVideoPath = Path.Combine(Path.GetDirectoryName(videoPath)!, _testCommonFileName + _videoExtension);
                // Wait for file to be free to rename it
                using var cts = new CancellationTokenSource(8000);
                while (!cts.IsCancellationRequested)
                {
                    try
                    {
                        File.Move(videoPath, newVideoPath);
                        break;
                    }
                    catch (IOException)
                    {
                        await Task.Delay(800, cts.Token);
                    }
                }
                TestContext.AddTestAttachment(newVideoPath, "Playwright Video");
            }
        }
    }
    internal static void SetEnvironmentVariable(string varaible, object? value, string prefix = _prefix) => SetEnvironmentVariable(varaible, value?.ToString(), prefix);
    internal static void SetEnvironmentVariable(string variable, string? value, string prefix = _prefix) => System.Environment.SetEnvironmentVariable(prefix + variable, value);
    private static string? GetEnvironmentVariable(string variable, string prefix = _prefix) => System.Environment.GetEnvironmentVariable(prefix + variable);
    internal static string? GetEnvVarValueFor<T>(Expression<Func<T, object?>> expression) => GetEnvironmentVariable(GetEnvVarNameFor(expression));
    internal static string GetEnvVarNameFor<T>(Expression<Func<T, object?>> expression)
    {
        var body = expression.Body;

        // Unbox if necessary
        if (body is UnaryExpression unary && unary.NodeType == ExpressionType.Convert && unary.Operand is MemberExpression)
        {
            body = unary.Operand;
        }

        if (body is MemberExpression member && typeof(T).FullName is string fullName)
        {
            var key = $"{fullName.Replace('.', '_').ToUpperInvariant()}_{member.Member.Name.ToUpperInvariant()}";
            return key;
        }

        throw new ArgumentException("Expression is not a member access", nameof(expression));
    }
    internal static async Task<(IPlaywright, IBrowser)> OneTimeSetUpAsync()
    {
        SetEnvironmentVariable("BROWSER", BrowserType.Chromium, string.Empty);
        SetEnvironmentVariable("DEBUG", "pw:api", string.Empty);
        SetEnvironmentVariable("HEADED", 1, string.Empty);
        SetEnvironmentVariable("PWDEBUG", 0, string.Empty);
        SetEnvironmentVariable(GetEnvVarNameFor<TracingStartOptions>(o => o.Screenshots), true);
        SetEnvironmentVariable(GetEnvVarNameFor<TracingStartOptions>(o => o.Snapshots), true);
        SetEnvironmentVariable(GetEnvVarNameFor<TracingStartOptions>(o => o.Sources), true);
        SetEnvironmentVariable(GetEnvVarNameFor<BrowserNewContextOptions>(o => o.RecordVideoDir), AssetsDirectory);
        SetEnvironmentVariable(GetEnvVarNameFor<TracingStopOptions>(o => o.Path), AssetsDirectory);
        SetEnvironmentVariable(GetEnvVarNameFor<ViewportSize>(o => o.Width), 1280);
        SetEnvironmentVariable(GetEnvVarNameFor<ViewportSize>(o => o.Height), 720);
        SetEnvironmentVariable(GetEnvVarNameFor<HttpCredentials>(o => o.Username), "username");
        SetEnvironmentVariable(GetEnvVarNameFor<HttpCredentials>(o => o.Password), "password");

        var playwright = await Playwright.CreateAsync();
        var browser = await playwright[PlaywrightSettingsProvider.BrowserName].LaunchAsync(PlaywrightSettingsProvider.LaunchOptions).ConfigureAwait(false);
        return (playwright, browser);
    }
    internal static async Task OneTimeTearDownAsync(IPlaywright playwright, IBrowser browser)
    {
        await browser.CloseAsync();
        playwright.Dispose();
    }
}

public sealed class ScopedTestContext
{
    public readonly IBrowserContext Context;
    public readonly IPage Page;
    public ScopedTestContext(IBrowserContext context, IPage page)
    {
        Context = context;
        Page = page;
    }

}

public static class TracingStartOptionsExtensions
{
    public static bool ShouldStartOrStopTracing(this TracingStartOptions? options) => options != null && (options.Screenshots == true || options.Snapshots == true || options.Sources == true);
}
