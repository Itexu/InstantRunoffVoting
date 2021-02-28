
namespace IRVTest
{
    using InstantRunoffVoting;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            var lBallot = new Ballot()
            {
                Name = "Test",
                Threshold = 60
            };

            lBallot.AddNewChoice("Radio 10");
            lBallot.AddNewChoice("Veronica");
            lBallot.AddNewChoice("Q Music");
            lBallot.AddNewChoice("538");
            lBallot.AddNewChoice("Arrow");

            GenerateRandomVotes(lBallot, 20);

            var lVote = lBallot.GetVoteForVoter("Test");

            var lStop = false;
            var lDirectionUp = true;
            Choice[] lCurrentRanking;
            int i = 0;
            while (!lStop)
            {
                lCurrentRanking = lVote.GetCurrentRanking();
                ShowVoteMenu(lCurrentRanking);

                var s = Console.ReadLine();

                switch (s?.ToLowerInvariant())
                {
                    case "q":
                        lStop = true;
                        break;
                    case "u":
                        lDirectionUp = true;
                        break;
                    case "d":
                        lDirectionUp = false;
                        break;
                    case "s":
                        lBallot.SubmitVote(lVote);
                        lVote = lBallot.GetVoteForVoter("Test"+i);
                        i++;
                        break;
                    case "r":
                        GenerateRandomVotes(lBallot, 20);
                        break;
                    case "c":
                        ShowResult(lBallot.CalcResult());
                        break;
                    default:
                        if (int.TryParse(s, out int lInt))
                        {
                            if (lDirectionUp)
                                lVote.IncreaseChoiceRank(lCurrentRanking[lInt].UniqueID);
                            else
                                lVote.DecreaseChoiceRank(lCurrentRanking[lInt].UniqueID);
                        }
                        break;
                }
            }
        }

        private static void ShowVoteMenu(Choice[] pChoices)
        {
            Console.Clear();

            for (int i = 0; i < pChoices.Length; i++)
            {
                Console.WriteLine("{0,2}{1,20}{2,40}", i, pChoices[i].Name, pChoices[i].UniqueID);
            }

            Console.WriteLine();
        }

        private static void ShowResult(Dictionary<int, Dictionary<Choice, List<Vote>>> pResult)
        {
            Console.Clear();

            foreach (var fRound in pResult)
            {
                var OrderedResult = fRound.Value.OrderByDescending(o => o.Value.Count());

                Console.WriteLine("Round " + fRound.Key);
                foreach (var fResult in OrderedResult)
                {
                    Console.WriteLine("{0,20}{1,4}", fResult.Key?.Name, fResult.Value?.Count);
                }

                Console.WriteLine();
                Console.WriteLine("-----------------------------------------");
            }

            Console.ReadKey();
        }

        private static void GenerateRandomVotes(Ballot pBallot, int pNoOfVotes)
        {
            Random lRandom = new Random();

            for (int i = 0; i < pNoOfVotes; i++)
            {
                var lVote = pBallot.GetVoteForVoter("Random" + i);

                var lRanking = new List<Choice>(lVote.GetCurrentRanking());

                int j = 0;
                while (lRanking.Count > 0)
                {
                    var r = lRandom.Next(lRanking.Count );

                    lVote.SetRank(lRanking[r].UniqueID, j);
                    lRanking.RemoveAt(r);
                    j++;
                }

                pBallot.SubmitVote(lVote);
            }
        }
    }
}
