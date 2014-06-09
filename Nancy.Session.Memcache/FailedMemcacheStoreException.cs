
using System;
using System.Runtime.Serialization;

namespace Nancy.Session.Memcache
{
    [Serializable]
    public class FailedMecacheStoreException : Exception
    {
        public FailedMecacheStoreException() { }
        public FailedMecacheStoreException(string message) : base(message) { }
        public FailedMecacheStoreException(string message, Exception innerException) : base(message, innerException) { }
        protected FailedMecacheStoreException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}