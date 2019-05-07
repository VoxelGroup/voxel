using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyWebSockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VotingApp.Lib;

namespace VotingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotingController : ControllerBase
    {
        private readonly Voting _voting;
        private readonly int _step;
        private readonly ILogger<VotingController> _logger;
        private readonly IWebSocketPublisher _wsPublisher;

        public VotingController(Voting voting, IConfiguration config, ILogger<VotingController> logger, IWebSocketPublisher wsPublisher)
        {
            _voting = voting;
            _step = config.GetValue<int>("VotingStep", 1);
            _logger = logger;
            _wsPublisher = wsPublisher;
        }

        [HttpGet]
        public object Get() => _voting.GetState();

        [HttpPost]
        public Task<object> Post([FromBody] string[] options) =>
            ExecuteCommand(() => _voting.Start(options));

        [HttpPut]
        public Task<object> Put([FromBody] string option) =>
            ExecuteCommand(() => _voting.Vote(option, _step));

        [HttpDelete]
        public Task<object> Delete() =>
            ExecuteCommand(_voting.Finish);

        private async Task<object> ExecuteCommand(Action command)
        {
            _logger.LogWarning($"Starting Command with {JsonConvert.SerializeObject(_voting.GetState())}");
            command();
            await _wsPublisher.SendMessageToAllAsync(_voting.GetState());
            _logger.LogWarning($"Finishing Command with {JsonConvert.SerializeObject(_voting.GetState())}");
            return _voting.GetState();
        }
    }
}
