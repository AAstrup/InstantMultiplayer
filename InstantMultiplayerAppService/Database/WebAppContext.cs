using InstantMultiplayerAppService.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace InstantMultiplayerAppService.Database
{
    public class WebAppContext : DbContext
    {
        public WebAppContext(DbContextOptions<WebAppContext> context) : base(context)
        {

        }

        public DbSet<EmailSignup> EmailSignups { get; set; }
    }
}