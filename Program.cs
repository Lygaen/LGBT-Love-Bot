using System;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace LGBT_Love_Bot
{
    class Program
    {
        private static DiscordClient _client;

        static void Main()
        {
            dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(@".\config.json"));
            Fields.channel = json?.channel;
            Fields.message = json?.message;
            Fields.token = json?.token;
            Fields.partenariat = json?.partenariat;
            Fields.report = json?.report;
            Fields.help = json?.help;
            Fields.question = json?.question;
            Login().GetAwaiter().GetResult();
        }

        static async Task Login()
        {
            _client = new DiscordClient(new DiscordConfiguration()
            {
                Token = Fields.token,
                TokenType = TokenType.Bot,
                MinimumLogLevel = LogLevel.None
            });

            Console.WriteLine("Setting up Interactivity...");
            
            _client.Ready += Events.Ready;
            _client.MessageReactionAdded += Events.ReactionAdded;

            var cmds = _client.UseCommandsNext(new CommandsNextConfiguration()
            {
                CaseSensitive = true,
                DmHelp = false,
                EnableMentionPrefix = true,
                StringPrefixes = new []{@"\"}
            });
            
            cmds.RegisterCommands<Commands>();

            Console.WriteLine("Logging In !");
            await _client.ConnectAsync(new DiscordActivity("Démarre...", ActivityType.Playing));

            await Task.Delay(-1);
        }
    }
}