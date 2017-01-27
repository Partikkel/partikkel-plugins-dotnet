using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PartikkelDemo.Startup))]
namespace PartikkelDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
