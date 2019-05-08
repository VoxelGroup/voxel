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
        private readonly IVotingService _votingService;
        private readonly int _step;
        private readonly ILogger<VotingController> _logger;
        private readonly IWebSocketPublisher _wsPublisher;

        public VotingController(IVotingService votingService, IConfiguration config, ILogger<VotingController> logger, IWebSocketPublisher wsPublisher)
        {
            _votingService = votingService;
            _step = config.GetValue<int>("VotingStep", 1);
            _logger = logger;
            _wsPublisher = wsPublisher;
        }

        [HttpGet]
        public async Task<object> Get()
        {
            var voting = await _votingService.Get();
            return voting.GetState();
        }

        [HttpPost]
        public Task<object> Post([FromBody] string[] options) =>
            ExecuteCommand(voting => voting.Start(options));

        [HttpPut]
        public Task<object> Put([FromBody] string option) =>
            ExecuteCommand(voting => voting.Vote(option, _step));

        [HttpDelete]
        public async Task<object> Delete()
        {
            var result = await ExecuteCommand(voting => voting.Finish());
            Common.Logger.SaveLog("mylegacylog.xml");
            return result;
        }

        private async Task<object> ExecuteCommand(Action<Voting> command)
        {
            var voting = await _votingService.Get();
            _logger.LogWarning($"Starting Command with {JsonConvert.SerializeObject(voting.GetState())}");
            Common.Logger.LogInfo($"Starting Command with {JsonConvert.SerializeObject(voting.GetState())}");

            command(voting);
            await _votingService.Save(voting);
            await _wsPublisher.SendMessageToAllAsync(voting.GetState());

            _logger.LogWarning($"Finishing Command with {JsonConvert.SerializeObject(voting.GetState())}");
            return voting.GetState();
        }
    }
}
