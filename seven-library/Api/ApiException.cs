using System;

namespace seven_library.Api {
    [Serializable()]
    public class ApiException : Exception {
        public ApiException() {
        }

        public ApiException(string message) : base(message) {
        }

        public ApiException(string message, Exception inner) : base(message, inner) {
        }

        // Cnstructor needed for serialization when an exception propagates from a remoting server to the client.
        protected ApiException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) {
        }
    }
}