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
        /// <summary>
        /// Creates a new Vote for Voter.
        /// Will return old Vote if Found
        /// </summary>
        /// <param name="pVoter">Voter/User to get Vote for</param>
        /// <returns></returns>
        public Vote GetVoteForVoter(string pVoter)
        {
            return SubmittedVotes.Find(v => string.Equals(v.Voter, pVoter, StringComparison.OrdinalIgnoreCase)) ?? new Vote(pVoter, UniqueID, Choices);
        }

        /// <summary>
        /// Submit Vote
        /// </summary>
        /// <param name="pVote"></param>
        public void SubmitVote(Vote pVote)
        {
            if (VoteClosed != null)
                if (DateTime.UtcNow > VoteClosed.ToUniversalTime())
                    throw new ApplicationException("Vote Already Closed");

            var lOldVote = SubmittedVotes.Find(v => string.Equals(v.VoteID, pVote.VoteID));
            if (lOldVote != null)
                SubmittedVotes.Remove(lOldVote);
            lOldVote = SubmittedVotes.Find(v => string.Equals(v.Voter, pVote.Voter, StringComparison.OrdinalIgnoreCase));
            if (lOldVote != null)
                SubmittedVotes.Remove(lOldVote);

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
