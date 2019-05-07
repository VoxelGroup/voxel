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

        public Dictionary<string, int> Votes { get; set; }
        public string Winner { get; set; }

        public void Start(params string[] options)
        {
            Votes = options.ToDictionary(o => o, _ => 0);
        }

        public void Vote(string option)
        {
            Votes[option]++;
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
