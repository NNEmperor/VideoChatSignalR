using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class StartingChatData
    {
        public object UserToCall { get; set; }
        public object From { get; set; }
        public object Signal { get; set; }
        public object Name { get; set; } //name person who is calling
    }
}
