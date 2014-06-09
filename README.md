Nancy.Sessions.Memcache
=======================

Session provider for NancyFx using a memcache server for session storage.

Usage:

```csharp
public class Bootstrapper : DefaultNancyBootstrapper
{
    protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
    {
        base.ApplicationStartup(container, pipelines);
        var sessionManager = new SessionManager(
            new MemcacheSessionProvider(
                "SessionId",               // Cookie name
                "MyApplication-Sessions-"  // Memcache prefix
            )
        );
        sessionManager.Run(pipelines);
    }
}
```
