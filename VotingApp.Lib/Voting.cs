using System;
using System.Collections.Generic;
using System.Linq;

namespace VotingApp.Lib
{
    public class Voting
    {
        public Voting()
        {
        }

        public Voting(Dictionary<string, int> votes, string winner)
        {
            Votes = votes;
            Winner = winner;
        }

        public Dictionary<string, int> Votes { get; private set; }
        public string Winner { get; private set; }

        public void Start(params string[] options)
        {
            Votes = options.ToDictionary(o => o, _ => 0);
            Winner = "";
        }

        public void Vote(string option, int step = 1)
        {
            Votes[option] = Votes[option] + step;
        }


        public void Finish()
        {
            Winner = Votes.Aggregate((a, b) => a.Value > b.Value ? a : b).Key;
        }

        public object GetState() => new
        {
            Votes,
            Winner
        };
    }
}
