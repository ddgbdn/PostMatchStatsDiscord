using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordClient
{
    public class ServiceExtesnsions
    {
        public static IServiceProvider ConfigureServices()
        {
            var map = new ServiceCollection();
                // Repeat this for all the service classes
                // and other dependencies that your commands might need.
                //.AddSingleton(new todelete());

            // When all your required services are in the collection, build the container.
            // Tip: There's an overload taking in a 'validateScopes' bool to make sure
            // you haven't made any mistakes in your dependency graph.
            return map.BuildServiceProvider();
        }
    }
}
