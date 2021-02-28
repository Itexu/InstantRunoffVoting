
namespace IRVTest
{
    using InstantRunoffVoting;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Remoting.Metadata.W3cXsd2001;

    class Program
    {
        static void Main(string[] args)
        {
            var lBallot = new Ballot("Test Ballot");

            lBallot.AddNewChoice("Marc");
            lBallot.AddNewChoice("Nick");
            lBallot.AddNewChoice("Kelvin");
            lBallot.AddNewChoice("Jeroen");
            lBallot.AddNewChoice("Lieke");

            var lVote = lBallot.CreateVote();

            var lStop = false;
            var lDirectionUp = true;
            Choice[] lCurrentRanking;
            while (!lStop)
            {
                lCurrentRanking = lVote.GetCurrentRanking();
                ShowVoteMenu(lCurrentRanking);

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
                    case "s":
                        lBallot.SubmitVote(lVote);
                        lVote = lBallot.CreateVote();
                        break;
                    default:
                        if (int.TryParse(s, out int lInt))
                            if (lDirectionUp)
                                lVote.IncreaseChoiceRank(lCurrentRanking[lInt].UniqueID);
                            else
                                lVote.DecreaseChoiceRank(lCurrentRanking[lInt].UniqueID);
                        break;
                }
            }
        }

        private static void ShowVoteMenu(Choice[] pChoices)
        {
            Console.Clear();

            for (int i = 0; i < pChoices.Length; i++)
            {
                //Console.WriteLine(i + ": " + pChoices[i].Name + "\t\t(" + pChoices[i].UniqueID + ")");
                Console.WriteLine("{0,2}{1,20}{2,40}", i, pChoices[i].Name, pChoices[i].UniqueID);
            }

            Console.WriteLine();
        }
    }
}
