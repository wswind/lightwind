using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace castlecoresample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private IHelloRobot _hello;

        public TestController(IHelloRobot hello, ILogger<TestController> logger)
        {
            _logger = logger;
            _hello = hello;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            _hello.Hello();
            await _hello.Hello2();
            await _hello.Hello3();
            return "ok";            
        }
    }
}
