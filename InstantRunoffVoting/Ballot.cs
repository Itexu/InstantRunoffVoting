namespace InstantRunoffVoting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Ballot
    {
        public int UniqueID { get; }
        public string Name { get; set; }

        public DateTime VoteOpen { get; set; }
        public DateTime VoteClosed { get; set; }

        private readonly List<Choice> Choices;

        public Ballot(int pUniqueID, string pName, List<Choice> pChoices = null)
        {
            UniqueID = pUniqueID;
            Name = pName;

            Choices = pChoices ?? new List<Choice>();
        }

        public Vote CreateVote()
        {
            return new Vote(UniqueID, Choices);
        }

        public void AddNewChoice(string pName)
        {
            Choices.Add(new Choice(NewID(), pName));
        }

        private int NewID()
        {
            if (Choices.Count == 0)
                return 1;
            return Choices.OrderByDescending(c => c.UniqueID).FirstOrDefault().UniqueID + 1;
        }
    }
}
