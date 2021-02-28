namespace InstantRunoffVoting
{
    using System;
    using System.Collections.Generic;
    using System.IO.Pipes;

    public class Vote
    {
        public string BallotID { get; }
        public string VoteID { get; }
        private readonly Choice[] Choices;

        public Vote(string pBallotID, List<Choice> pChoices, string pVoteID = null)
        {
            VoteID = pVoteID;
            BallotID = pBallotID;
            Choices = pChoices?.ToArray() ?? throw new ArgumentNullException("Can't create empty Vote", nameof(pChoices));
            if (Choices.Length == 0)
                throw new ArgumentException("Can't create empty Vote", nameof(pChoices));
        }
        public Vote(string pBallotID, Choice[] pChoices, string pVoteID = null)
        {
            VoteID = pVoteID ?? Tools.CreateUniqueID();
            BallotID = pBallotID;
            Choices = pChoices ?? throw new ArgumentNullException("Can't create empty Vote", nameof(pChoices));
            if (Choices.Length == 0)
                throw new ArgumentException("Can't create empty Vote", nameof(pChoices));
        }

        #region Ranking
        public void IncreaseChoiceRank(string pChoiceID)
        {
            var lChoiceRank = GetChoiceCurrentRank(pChoiceID);
            if (lChoiceRank <= 0) // Not first
                return;

            SwitchPositions(lChoiceRank, lChoiceRank - 1);
        }

        public void DecreaseChoiceRank(string pChoiceID)
        {
            var lChoiceRank = GetChoiceCurrentRank(pChoiceID);
            if (lChoiceRank >= Choices.Length - 1) // not last
                return;

            SwitchPositions(lChoiceRank, lChoiceRank + 1);
        }

        public void SetRank(string pChoiceID, int pintNewRank)
        {
            int lCurrentRank = GetChoiceCurrentRank(pChoiceID);

            if (!CheckWithingRange(pintNewRank))
                throw new ArgumentException("Requested new rank, not in range of Vote", nameof(pintNewRank));

            if (pintNewRank == lCurrentRank)
                return;

            if (lCurrentRank < pintNewRank)
                for (int i = lCurrentRank; i <= pintNewRank; i++)
                {
                    DecreaseChoiceRank(pChoiceID);
                }
            else
                for (int i = lCurrentRank; i <= pintNewRank; i--)
                {
                    IncreaseChoiceRank(pChoiceID);
                }

        }

        private int GetChoiceCurrentRank(string pChoiceID)
        {
            for (int i = 0; i < Choices.Length; i++)
            {
                if (string.Equals(Choices[i].UniqueID , pChoiceID,StringComparison.OrdinalIgnoreCase))
                    return i;
            }

            throw new ArgumentException("Choice not found in vote", nameof(pChoiceID));
        }

        private void SwitchPositions(int pRank1, int pRank2)
        {
            if (!CheckWithingRange(pRank1) || !CheckWithingRange(pRank2))
                throw new IndexOutOfRangeException("Not all choices found in vote");


            var lTemp = Choices[pRank1];
            Choices[pRank1] = Choices[pRank2];
            Choices[pRank2] = lTemp;
        }

        private bool CheckWithingRange(int pRank)
        {
            return (0 <= pRank && pRank < Choices.Length);
        }
        #endregion Ranking

        public Choice[] GetCurrentRanking()
        {
            Choice[] lChoices = new Choice[Choices.Length];
            Choices.CopyTo(lChoices, 0);
            return lChoices;
        }
    }
}
