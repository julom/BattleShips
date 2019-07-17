using System;
using System.Runtime.Serialization;

namespace BattleShips.Core.Exceptions
{
    public class GameArgumentException : ArgumentException
    {
        public GameArgumentException()
        {
        }

        public GameArgumentException(string message) : base(message)
        {
        }

        public GameArgumentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public GameArgumentException(string message, string paramName) : base(message, paramName)
        {
        }

        public GameArgumentException(string message, string paramName, Exception innerException) : base(message, paramName, innerException)
        {
        }

        protected GameArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
