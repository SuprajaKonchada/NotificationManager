using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationManager
{
    /// <summary>
    /// Encapsulates details of a notification.
    /// </summary>
    public class NotificationEventArgs(string message) : EventArgs
    {
        public string Message { get; private set; } = message;
        public DateTime Timestamp { get; private set; } = DateTime.Now;
    }
}
