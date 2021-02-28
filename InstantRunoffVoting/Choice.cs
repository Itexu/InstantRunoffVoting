

namespace InstantRunoffVoting
{
    public class Choice
    {
        public string UniqueID { get; }
        public string Name { get; }


        public Choice(string pName, string pUniqueID = null)
        {
            Name = pName;
            UniqueID = pUniqueID ?? Tools.CreateUniqueID();
        }
    }
}
