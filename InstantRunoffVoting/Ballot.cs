namespace InstantRunoffVoting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Ballot
    {
        public string UniqueID { get; }
        public string Name { get; set; }

        public DateTime VoteOpen { get; set; }
        public DateTime VoteClosed { get; set; }

        private readonly List<Choice> Choices;

        private readonly List<Vote> SubmittedVotes;

        public Ballot(string pName, List<Choice> pChoices = null, string pUniqueID = null)
        {
            Name = pName;
            UniqueID = pUniqueID ?? Tools.CreateUniqueID();

            Choices = pChoices ?? new List<Choice>();
            SubmittedVotes = new List<Vote>();
        }

        #region Votes
        public Vote CreateVote()
        {

            return new Vote(UniqueID, Choices);
        }

        public void SubmitVote(Vote pVote)
        {
            /*if (SubmittedVotes.Count == 0) {
                SubmittedVotes.Add(pVote);
                return;
            }*/

            var lOldVOte = SubmittedVotes.Find(v => v.VoteID == pVote.VoteID);
            if (lOldVOte != null)
                SubmittedVotes.Remove(lOldVOte);

            SubmittedVotes.Add(pVote);
        }
        #endregion Votes

        #region Choices
        public void AddNewChoice(string pName)
        {
            Choices.Add(new Choice(pName));
        }
        #endregion Choices
    }
}
