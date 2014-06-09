using Enyim.Caching;

namespace Nancy.Sessions.Memcache
{
    public interface IMemcacheClientFactory
    {
        MemcachedClient NewClient();
    }
}
