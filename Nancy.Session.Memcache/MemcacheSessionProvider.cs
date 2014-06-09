using System;
using System.Collections.Generic;
using Enyim.Caching.Memcached;
using Nancy.Cookies;
using Newtonsoft.Json;

namespace Nancy.Session.Memcache
{
    public class MemcacheSessionProvider : ISessionProvider
    {
        private string MemcachePrefix { get; set; }
        private string CookieName { get; set; }
        private IMemcacheClientFactory MemcacheFactory { get; set; }


        public MemcacheSessionProvider(string cookieName, string memcachePrefix)
        {
            CookieName = cookieName;
            MemcachePrefix = memcachePrefix;
            MemcacheFactory = new BasicMemcacheClientFactory();
        }

        public MemcacheSessionProvider(string cookieName, string memcachePrefix, IMemcacheClientFactory factory)
        {
            CookieName = cookieName;
            MemcachePrefix = memcachePrefix;
            MemcacheFactory = factory;
        }


        public ISession Load(NancyContext context)
        {
            var token = MemcacheTokenFromCookie(context);
            if (String.IsNullOrEmpty(token))
            {
                var session = new Session();
                OnSessionStart(session);
            }
            return new Session(LoadFromMemcache(token));
        }

        public IDictionary<string, object> LoadFromMemcache(string key)
        {
            using (var client = MemcacheFactory.NewClient())
            {
                var result = client.ExecuteGet<string>(key);
                if (!result.Success)
                {
                    return new Dictionary<string, object>();
                }
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.Value);
                if (dictionary == null)
                {
                    return new Dictionary<string, object>();
                }
                return dictionary;
            }
        }

        public void Save(NancyContext context)
        {
            var session = context.Request.Session;
            if (session == null || !session.HasChanged)
            {
                return;
            }

            var token = MemcacheTokenFromCookie(context);
            if (string.IsNullOrEmpty(token))
            {
                var sessionId = NewSessionId();
                var cookie = new NancyCookie(CookieName, sessionId, true);
                context.Response.Cookies.Add(cookie);
                token = string.Concat(MemcachePrefix, sessionId);
            }

            var dictionary = new Dictionary<string, object>(session.Count);
            foreach (var kvp in session)
            {
                dictionary.Add(kvp.Key, kvp.Value);
            }
            SaveToMemcache(token, dictionary);
        }

        private void SaveToMemcache(string token, IDictionary<string, object> dictionary)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new EmptyMemcacheTokenException("No token was specified during a save operation.");
            }

            using (var client = MemcacheFactory.NewClient())
            {
                var serializedData = JsonConvert.SerializeObject(dictionary);
                var result = client.ExecuteStore(StoreMode.Set, token, serializedData);
                if (!result.Success)
                {
                    throw new FailedMecacheStoreException(String.Format("Unable to store data: {0}", result.Message));
                }
            }
        }

        private string NewSessionId()
        {
            return Guid.NewGuid().ToString();
        }

        public event Action<ISession> SessionEnd;

        protected virtual void OnSessionEnd(ISession obj)
        {
            var handler = SessionEnd;
            if (handler != null)
            {
                handler(obj);
            }
        }

        public event Action<ISession> SessionStart;
        protected virtual void OnSessionStart(ISession obj)
        {
            var handler = SessionStart;
            if (handler != null)
            {
                handler(obj);
            }
        }

        private string MemcacheTokenFromCookie(NancyContext context)
        {
            if (context.Request.Cookies.ContainsKey(CookieName))
            {
                return string.Concat(MemcachePrefix, context.Request.Cookies[CookieName]);
            }
            return null;
        }

        public bool Expired(NancyContext context)
        {
            throw new NotImplementedException();
        }
    }
}
