using System.Threading.Tasks;
using VotingApp.Lib;

namespace VotingApp.Api
{
    public class InMemoryVotingService : IVotingService
    {
        private readonly Voting _voting = new Voting();

        public Task Save(Voting voting) => Task.FromResult(true);

        public Task<Voting> Get() => Task.FromResult(_voting);
    }
}