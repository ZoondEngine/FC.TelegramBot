﻿using System;
using System.Diagnostics;

namespace Extensions.Object.Exceptions
{
    public class ExException : Exception
    {
        private readonly StackTrace m_StackTrace;
        public ExException( string message )
            : base( message )
        {
            m_StackTrace = new StackTrace( this, true );
        }

        public int ExceptionLine()
        {
            var frame = m_StackTrace.GetFrame( 0 );
            if ( frame != null )
            {
                return frame.GetFileLineNumber();
            }

            return 0;
        }
        public StackTrace GetStackTrace()
            => m_StackTrace;
        public bool IsBaseOf<T>() where T : ExException
            => this is T;
    }
}
