using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMultiplayerAppService.Database;
using InstantMultiplayerAppService.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InstantMultiplayerAppService.Controllers
{
    [ApiController]
    [Route("Email")]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;
        private readonly WebAppContext _context;

        public EmailController(ILogger<EmailController> logger, WebAppContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("index")]
        public int Index()
        {
            return _context.EmailSignups.Count() + 100000;
        }

        [HttpPost("add/{email}")]
        public async Task<string> Add([FromRoute] string email)
        {
            var ip = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();
            await _context.EmailSignups.AddAsync(new EmailSignup() { Email = email, Ip = ip });
            await _context.SaveChangesAsync();
            return "SAVED:" + ip;
        }
    }
}
