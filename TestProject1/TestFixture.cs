using FlipCardProject.Services;
using Microsoft.Extensions.DependencyInjection;

namespace TestProject1;

public class TestFixture : IDisposable
{
    public IServiceProvider ServiceProvider { get; }

    public TestFixture()
    {
        var services = new ServiceCollection();

        
        services.AddTransient<UserTrackingService<int>>();
        services.AddTransient<FlipcardSetValidator>();

        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
