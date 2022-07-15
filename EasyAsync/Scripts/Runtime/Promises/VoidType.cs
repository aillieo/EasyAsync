using System;

namespace AillieoUtils.EasyAsync
{
    public struct VoidType : IEquatable<VoidType>
    {
        public bool Equals(VoidType other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is VoidType;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
