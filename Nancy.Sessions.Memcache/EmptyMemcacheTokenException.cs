
using System;
using System.Runtime.Serialization;

namespace Nancy.Sessions.Memcache
{
    [Serializable]
    public class EmptyMemcacheTokenException : Exception
    {
        public EmptyMemcacheTokenException() { }
        public EmptyMemcacheTokenException(string message) : base(message) { }
        public EmptyMemcacheTokenException(string message, Exception innerException) : base(message, innerException) { }
        protected EmptyMemcacheTokenException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
