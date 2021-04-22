using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.Object
{
    public static class ExUnsafe
    {
        public static T Convert<T>( this object obj )
            => ( T )obj;
    }
}
