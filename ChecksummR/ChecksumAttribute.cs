using System;

namespace ChecksummR
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct)]
    public class ChecksumAttribute : Attribute
    {
        public ChecksumType ChecksumType { get; set; }

        public ChecksumAttribute(ChecksumType type)
        {
            ChecksumType = type;
        }
    }
}
