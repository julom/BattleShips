﻿using System;
using System.Runtime.Serialization;

namespace BattleShips.Core.Exceptions
{
    public class GameException : Exception
    {
        public GameException()
        {
        }

        public GameException(string message) : base(message)
        {
        }

        public GameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
