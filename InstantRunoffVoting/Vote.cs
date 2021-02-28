using System;
using System.Collections.Generic;

namespace InstantRunoffVoting
{
    public class Vote
    {
        public int BallotID { get; }
        private readonly List<Choice> Choices;

        public Vote(int pBallotID, List<Choice> pChoices)
        {
            BallotID = pBallotID;
            Choices = pChoices ?? throw new ArgumentNullException("Can't create empty Vote", nameof(pChoices));
            if (Choices.Count == 0)
                throw new ArgumentException("Can't create empty Vote", nameof(pChoices));
        }
    }
}
