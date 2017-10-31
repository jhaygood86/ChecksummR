using System;
using System.Collections.Generic;
using System.Text;

namespace ChecksummR
{
    public class ChecksumPropertyAttribute : Attribute
    {
        public bool IncludeProperty = true;
    }
}
