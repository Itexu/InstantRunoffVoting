
namespace IRVTest
{
    using InstantRunoffVoting;
    using System;
    using System.Runtime.Remoting.Metadata.W3cXsd2001;

    class Program
    {
        static void Main(string[] args)
        {
            var lBallot = new Ballot(0, "Test Ballot");

            lBallot.AddNewChoice("Marc");
            lBallot.AddNewChoice("Nick");
            lBallot.AddNewChoice("Kelvin");
            lBallot.AddNewChoice("Jeroen");
            lBallot.AddNewChoice("Lieke");

            var lVote = lBallot.CreateVote();

            var lStop = false;
            var lDirectionUp = true;
            while (!lStop)
            {
                ShowVoteMenu(lVote.GetCurrentRanking());

                var s = Console.ReadLine();

                switch (s?.ToLowerInvariant())
                {
                    case "q":
                        lStop = false;
                        break;
                    case "u":
                        lDirectionUp = true;
                        break;
                    case "d":
                        lDirectionUp = false;
                        break;
                    default:
                        if (int.TryParse(s, out int lInt))
                            if (lDirectionUp)
                                lVote.IncreaseChoiceRank(lInt);
                            else
                                lVote.DecreaseChoiceRank(lInt);
                        break;
                }
            }
        }

        private static void ShowVoteMenu(Choice[] pChoices)
        {
            Console.Clear();

            foreach (var item in pChoices)
            {
                Console.WriteLine(item.UniqueID + ": " + item.Name);
            }

            Console.WriteLine();
        }
    }
}
