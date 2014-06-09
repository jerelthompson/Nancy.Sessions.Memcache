
using Enyim.Caching;

namespace Nancy.Session.Memcache
{
    class BasicMemcacheClientFactory : IMemcacheClientFactory
    {

        /// <summary>
        /// This is a very basic implementation of an IMemcachedClient factory.  If you are using
        /// some sort of IoC, you should implement your own factory which pulls it from there or
        /// if you wish to customize your client initialization.
        /// </summary>
        /// <returns></returns>
        public Enyim.Caching.MemcachedClient NewClient()
        {
            return new MemcachedClient();
        }
    }
}
