Nancy.Sessions.Memcache
=======================

Overview
--------

Session provider for [NancyFx](http://nancyfx.org/) using a memcache server for session storage.  This makes use of the
[EnyimMemcached](http://www.nuget.org/packages/EnyimMemcached/) client as well as [Json.NET](http://www.nuget.org/packages/Newtonsoft.Json/)
for serializing the data.

Usage
-----

Start by modifying your Web.config or App.config to specify your Memcached server by adding the following sections.

```csharp
<configSections>
    <sectionGroup name="enyim.com">
        <section name="memcached" type="Enyim.Caching.Configuration.MemcachedClientSection, Enyim.Caching" />
    </sectionGroup>
</configSections>
```

```csharp
<enyim.com>
    <memcached protocol="Binary">
        <servers>
            <!-- make sure you use the same ordering of nodes in every configuration you have -->
		    <add address="ip address" port="port number" />
		</servers>
	</memcached>
</enyim.com>
```

Finally, create a new Bootstrapper class for your application that will setup the new session manager which
uses the MemcacheSessionProvider.


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
