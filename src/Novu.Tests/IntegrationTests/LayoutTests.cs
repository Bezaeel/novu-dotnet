using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Novu.Tests.IntegrationTests;

public class LayoutTests : BaseIntegrationTest
{
    public LayoutTests(ITestOutputHelper output) : base(output)
    {
    }

    /// <summary>
    ///     This is a flaky test because 'default' layout operates globally to the application and can
    ///     conflict with other tests when running in parallel.
    /// </summary>
    [Fact]
    public async Task Should_SetStatus()
    {
        var layouts = await Layout.Get();
        var originalDefault = layouts.Data.Single(x => x.IsDefault);

        var layout = layouts.Data.First(x => !x.IsDefault);
        await Layout.SetAsDefault(layout.Id);
        var result = await Layout.Get(layout.Id);
        result.Data.IsDefault.Should().BeTrue();

        // cleanup to reinstate the original
        await Layout.SetAsDefault(originalDefault.Id);
    }
}