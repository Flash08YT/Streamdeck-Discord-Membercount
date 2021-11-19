using DcMemberCount.Models;
using StreamDeckLib;
using StreamDeckLib.Messages;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using DcMemberCount.Json;

namespace DcMemberCount
{
    [ActionUuid(Uuid = "de.flash.streamdeck.disordcounter")]
    public class Actions : BaseStreamDeckActionWithSettingsModel<SettingsModel>
    {
        private Timer timer;

        public override async Task OnKeyUp(StreamDeckEventPayload args)
        {
            string path = SettingsModel.Launchpath;

            var process = new Process
            {
                StartInfo =
              {
                  FileName = path,
                  Arguments = "--processStart Discord.exe"
              }
            };
            process.Start();

            startCount(args);

            //update settings
            await Manager.SetSettingsAsync(args.context, SettingsModel);
        }

        public override async Task OnDidReceiveSettings(StreamDeckEventPayload args)
        {
            await base.OnDidReceiveSettings(args);
            startCount(args);

        }

        public override async Task OnWillAppear(StreamDeckEventPayload args)
        {
            await base.OnWillAppear(args);
            startCount(args);

        }

        private void startCount(StreamDeckEventPayload args)
        {
                timer = new Timer(
                   e => updateCount(args),
                   null,
                   TimeSpan.Zero,
                   TimeSpan.FromMinutes(1));

        }

        private void updateCount(StreamDeckEventPayload args)
        {
            HttpClient client = new HttpClient();
            string url = "https://discord.com/api/guilds/" +  SettingsModel.WidgetUrl + "/widget.json";

            var res = client.GetAsync(url).GetAwaiter().GetResult();
            DiscordModel dcmodel = Newtonsoft.Json.JsonConvert.DeserializeObject<DiscordModel>(res.Content.ReadAsStringAsync().GetAwaiter().GetResult());

            Manager.SetTitleAsync(args.context, dcmodel.members.Count.ToString());
        }

    }
}
