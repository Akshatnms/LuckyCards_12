using System;
using System.Collections.Generic;

namespace Receivable
{
    public class Notification
    {
        public int id;
        public string sender;
        public string reciever;
        public int points;
        public DateTime created_at;
    }


    [Serializable]
    public class ReceivableData
    {
        public List<Notification> notification { get; set; }
        public int notification_count { get; set; }
    }

    [Serializable]
    public class Accept
    {
        public string sender;
        public string user;
        public string notify_id;
        public string device;

    }
    [Serializable]
    public class Reject
    {
        public string sender;
        public string reciever;
        public string user_id;
        public string notify_id;
        public string device;

    }

}
