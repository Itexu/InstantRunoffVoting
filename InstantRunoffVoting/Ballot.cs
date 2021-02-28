namespace InstantRunoffVoting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Ballot
    {
        public string UniqueID { get; }
        public string Name { get; set; }

        private decimal _threshold = 50;
        public decimal Threshold
        {
            get { return _threshold; }
            set {
                if (value >= 100)
                    _threshold = 99.999M;
                else
                    _threshold = value;
            }
        }

        public DateTime VoteOpen { get; set; }
        public DateTime VoteClosed { get; set; }

        private readonly List<Choice> Choices;

        private readonly List<Vote> SubmittedVotes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUniqueID">ID of Ballot</param>
        /// <param name="pChoices">Choices in Ballot</param>
        public Ballot(string pUniqueID = null, List<Choice> pChoices = null)
        {
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
            if (VoteClosed != null && VoteClosed > DateTime.MinValue)
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

        /// <summary>
        /// CalcResult of ballot
        /// Returns dictionary, results per round
        /// </summary>
        /// <returns>Round,Results</returns>
        public Dictionary<int, Dictionary<Choice, List<Vote>>> CalcResult()
        {
            var lResults = new Dictionary<int, Dictionary<Choice, List<Vote>>>();

            var lRemainingChoices = new List<Choice>();
            foreach (var c in Choices)
            {
                lRemainingChoices.Add(c);
            }

            int VoteThreshold = GetBallotThreshold(SubmittedVotes.Count());

            int i = 1;
            do
            {
                var lRound = CountRound(lRemainingChoices, SubmittedVotes);
                lResults.Add(i, lRound);

                var lOrderedRound = lRound.OrderByDescending(r => r.Value.Count());

                if (lOrderedRound.First().Value.Count() >= VoteThreshold)
                    return lResults;

                lRemainingChoices.Remove(lOrderedRound.Last().Key);

                i++;
            } while (lRemainingChoices.Count > 1);

            return lResults;
        }

        /// <summary>
        /// Place all votes under correct Choice
        /// </summary>
        /// <param name="pAvailableChoices">Choices still in running</param>
        /// <param name="pVotes"></param>
        /// <returns>Grouped votes</returns>
        private static Dictionary<Choice, List<Vote>> CountRound(List<Choice> pAvailableChoices, List<Vote> pVotes)
        {
            var pRoundResult = new Dictionary<Choice, List<Vote>>();

            foreach (var c in pAvailableChoices)
            {
                pRoundResult.Add(c, new List<Vote>());
            }

            foreach (var v in pVotes)
            {
                var lRanking = v.GetCurrentRanking();

                for (int i = 0; i < lRanking.Length; i++)
                {
                    var lChoice = pRoundResult.Keys.FirstOrDefault(c => string.Equals(c.UniqueID, lRanking[i].UniqueID, StringComparison.OrdinalIgnoreCase));
                    if (lChoice != null)
                    {
                        pRoundResult[lChoice].Add(v);
                        break;
                    }
                }
            }

            return pRoundResult;
        }

        private int GetBallotThreshold(int pTotalVotes)
        {
            return decimal.ToInt32(pTotalVotes * Threshold / 100m);
        }
    }
}
