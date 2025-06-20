using VContainer;
using VContainer.Unity;
using MessagePipe;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        var options = builder.RegisterMessagePipe();
        builder.RegisterBuildCallback(c =>
        {
            GlobalMessagePipe.SetProvider(c.AsServiceProvider());
        });
    }
}
