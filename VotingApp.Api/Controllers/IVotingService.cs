using System.Threading.Tasks;
using VotingApp.Lib;

namespace VotingApp.Api
{
    public interface IVotingService
    {
        Task Save(Voting voting);

        Task<Voting> Get();
    }
}