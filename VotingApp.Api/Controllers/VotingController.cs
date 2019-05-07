using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public VotingController(Voting voting, IConfiguration config, ILogger<VotingController> logger)
        {
            _voting = voting;
            _step = config.GetValue<int>("VotingStep", 1);
            _logger = logger;
        }

        [HttpGet]
        public object Get()
        {
            return _voting.GetState();
        }

        [HttpPost]
        public object Post([FromBody] string[] options)
        {
            _voting.Start(options);
            _logger.LogWarning($"Start Voting {JsonConvert.SerializeObject(_voting.GetState())}");
            return _voting.GetState();
        }

        [HttpPut]
        public object Put([FromBody] string option)
        {
            _voting.Vote(option, _step);
            return _voting.GetState();
        }

        [HttpDelete]
        public object Delete()
        {
            _voting.Finish();
            return _voting.GetState();
        }
    }
}
