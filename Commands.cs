using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace LGBT_Love_Bot
{
    public class Commands : BaseCommandModule
    {
        [Command("send")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task Send(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            embed.Title = "Ouvrez un ticket !";
            embed.Description = @"
🛡 -> Report
🌐 -> Aide
❓ -> Question(s)
🔒 -> Partenariat

⚠ Tout abus sera punis ! 
";
            embed.Color = DiscordColor.Green;
            var msg = await ctx.RespondAsync(embed:embed);
            await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":shield:"));
            await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":globe_with_meridians:"));
            await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":question:"));
            await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":lock:"));
        }
        
        [Command("credits")]
        public async Task Credits(CommandContext ctx)
        {
            await ctx.RespondAsync(embed:new DiscordEmbedBuilder()
            {
                Title = "Créer par Lygaen#7152",
                Description = "Contactez-moi à n'importe quel moment ! (Pas sûr que je réponde TOUT le temps par contre 😋)"
            });
        }
    }
}