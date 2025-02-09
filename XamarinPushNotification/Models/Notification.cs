using System.Collections.Generic;

namespace XamarinPushNotification.Models
{
    public class Notification
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public List<string> Tokens { get; set; }
        public Dictionary<string, string> CustomData { get; set; }
    }
}
