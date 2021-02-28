

namespace InstantRunoffVoting
{
    using System;

    public class Choice
    {
        public int UniqueID { get; }
        public string Name { get; }

        public Choice (int pintUniqueID, string ptxtName)
        {
            UniqueID = pintUniqueID;
            Name = ptxtName;
        }
    }
}
