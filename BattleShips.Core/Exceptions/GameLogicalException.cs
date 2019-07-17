using System;
using System.Runtime.Serialization;

namespace BattleShips.Core.Exceptions
{
    public class GameLogicalException : GameException
    {
        public GameLogicalException()
        {
        }

        public GameLogicalException(string message) : base(message)
        {
        }

        public GameLogicalException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GameLogicalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
