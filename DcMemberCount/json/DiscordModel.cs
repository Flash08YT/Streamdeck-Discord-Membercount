using System.Collections.Generic;

namespace DcMemberCount.Json
{
    public class DiscordModel
    {
        public string presence_count { get; }
        public List<object> members { get; }

        public DiscordModel(List<object> members, string presence_count)
        {
            this.presence_count = presence_count;
            this.members = members;

        }

    }
}
