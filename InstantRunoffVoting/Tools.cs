
using System;

namespace InstantRunoffVoting
{
    public static class Tools
    {
        public static string CreateUniqueID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
