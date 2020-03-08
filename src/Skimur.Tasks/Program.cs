using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PowerArgs;
using Skimur.Common;
using Skimur.Common.Utils;
using Skimur.Data;


namespace Skimur.Tasks
{
    class Program : IRegistrar
    {
        static void Main(string[] args)
        {
            SkimurContext.Initialize(new Program(), new DataRegistrar());

            Args.InvokeAction<CommandHandler>(args);
        }

        public int Order => 0;

        public void Register(IServiceCollection services)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            builder.AddEnvironmentVariables();

            services.AddSingleton<IConfiguration>(builder.Build());
        }
    }
}
