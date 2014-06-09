using Enyim.Caching;

namespace Nancy.Session.Memcache
{
    public interface IMemcacheClientFactory
    {
        MemcachedClient NewClient();
    }
}
