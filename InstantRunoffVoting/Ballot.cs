namespace InstantRunoffVoting
{
    using System;
    using System.Collections.Generic;

    public class Ballot
    {
        public int UniqueID { get; }
        public string Name { get; set; }

        public DateTime VoteFrom { get; set; }
        public DateTime VoteTill { get; set; }

        public List<Choice> Choices { get; }

        public Ballot(int pintUniqueID, string ptxtName, DateTime pdatFrom, DateTime pdatTill, List<Choice> pchoices = null)
        {
            UniqueID = pintUniqueID;
            Name = ptxtName;
            VoteFrom = pdatFrom;
            VoteTill = pdatTill;

            Choices = pchoices ?? new List<Choice>();
        }

        public Vote CreateVote()
        {
            return new Vote(UniqueID, Choices);
        }
    }
}
