using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace LGBT_Love_Bot
{
    public class Events
    {
        public static async Task Ready(DiscordClient sender, ReadyEventArgs e)
        {
            Console.WriteLine("Connected !");
            await sender.UpdateStatusAsync(new DiscordActivity("vous regarder...", ActivityType.Playing));
        }

        public static async Task ReactionAdded(DiscordClient sender, MessageReactionAddEventArgs e)
        {
            if (!(e.Channel.Id.ToString().Equals(Fields.channel) && e.Message.Id.ToString().Equals(Fields.message))) return;
            await e.Message.DeleteReactionAsync(e.Emoji, e.User);
            switch (e.Emoji.GetDiscordName())
            {
                case ":shield:":
                    await OpenReport(e);
                    break;
                case ":globe_with_meridians:":
                    await OpenHelp(e);
                    break;
                case ":question:":
                    await OpenQuestion(e);
                    break;
                case ":lock:":
                    await OpenPart(e);
                    break;
            }
        }

        private static async Task<DiscordChannel> CreateChannel(MessageReactionAddEventArgs e, string roleid)
        {
            var c = await e.Guild.CreateChannelAsync(e.User.Id.ToString(), ChannelType.Text);
            var member = await e.Guild.GetMemberAsync(e.User.Id);
            await c.AddOverwriteAsync(member, Permissions.SendMessages);
            await c.AddOverwriteAsync(member, Permissions.All);
            var role = e.Guild.GetRole(Convert.ToUInt64(roleid));
            await c.AddOverwriteAsync(role, Permissions.SendMessages);
            await c.AddOverwriteAsync(role, Permissions.All);
            await c.AddOverwriteAsync(e.Guild.EveryoneRole , deny:Permissions.All);
            return c;
        }
        
        private static async Task OpenPart(MessageReactionAddEventArgs e)
        {
            var check = await IsAlready(e);
            if (check) return;
            var c = CreateChannel(e, Fields.partenariat).GetAwaiter().GetResult();
            await c.SendMessageAsync(e.User.Mention, embed:new DiscordEmbedBuilder()
            {
                Title = "PARTENARIAT",
                Description = "Un des administrateurs/modérateurs prendra en charge votre demande ! Merci de bien détailer !",
                Color = DiscordColor.Green
            });
            Console.WriteLine($"Partenariat ticket with user {e.User.Username}#{e.User.Discriminator} opened !");
        }

        private static async Task OpenQuestion(MessageReactionAddEventArgs e)
        {
            var check = await IsAlready(e);
            if (check) return;
            
            var c = CreateChannel(e, Fields.question).GetAwaiter().GetResult();
            await c.SendMessageAsync(e.User.Mention, embed:new DiscordEmbedBuilder()
            {
                Title = "QUESTION",
                Description = "Vous avez ouvert un ticket pour une question, posez votre question et nous répondrons dès que possible !",
                Color = DiscordColor.Green
            });
            Console.WriteLine($"Question ticket with user {e.User.Username}#{e.User.Discriminator} opened !");
        }

        private static async Task OpenHelp(MessageReactionAddEventArgs e)
        {
            var check = await IsAlready(e);
            if (check) return;

            var c = CreateChannel(e, Fields.help).GetAwaiter().GetResult();
            await c.SendMessageAsync(e.User.Mention, embed:new DiscordEmbedBuilder()
            {
                Title = "HELP",
                Description = "Vous avez ouvert un ticket d'aide, demandez de l'aide et une personne vous aidera !",
                Color = DiscordColor.Green
            });
            Console.WriteLine($"Help ticket with user {e.User.Username}#{e.User.Discriminator} opened !");
        }

        private static async Task OpenReport(MessageReactionAddEventArgs e)
        {
            var check = await IsAlready(e);
            if (check) return;

            var c = CreateChannel(e, Fields.report).GetAwaiter().GetResult();
            await c.SendMessageAsync(e.User.Mention, embed:new DiscordEmbedBuilder()
            {
                Title = "REPORT",
                Description = "Vous avez ouvert un report, un modérateur ou un helper vous contactera dès que possible !",
                Color = DiscordColor.Green
            });
            Console.WriteLine($"Report ticket with user {e.User.Username}#{e.User.Discriminator} opened !");
        }

        private static async Task<bool> IsAlready(MessageReactionAddEventArgs e)
        {
            var channels = await e.Guild.GetChannelsAsync();
            return channels.FirstOrDefault(c => c.Name == e.User.Id.ToString()) != null;
        }
    }
}