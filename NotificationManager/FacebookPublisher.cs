using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationManager
{
    /// <summary>
    /// Publishes notifications and manages subscribers.
    /// </summary>
    public class FacebookPublisher
    {
        // Declare the event using EventHandler<T>
        public event EventHandler<NotificationEventArgs>? NotificationEvent;

        // Lock object for thread safety
        private readonly object _lock = new();

        // List to keep track of subscribers
        private readonly List<EventHandler<NotificationEventArgs>> _subscribers = [];

        /// <summary>
        /// Adds a subscriber to the notification event.
        /// </summary>
        /// <param name="subscriber">The event handler to add as a subscriber.</param>
        public void AddSubscriber(EventHandler<NotificationEventArgs> subscriber)
        {
            lock (_lock)
            {
                if (!_subscribers.Contains(subscriber))
                {
                    _subscribers.Add(subscriber);
                    NotificationEvent += subscriber;
                    Log($"Subscriber {subscriber.Method.Name} added.");
                }
                else
                {
                    Log($"Subscriber {subscriber.Method.Name} is already added.");
                }
            }
        }

        /// <summary>
        /// Removes a subscriber from the notification event.
        /// </summary>
        /// <param name="subscriber">The event handler to remove as a subscriber.</param>
        public void RemoveSubscriber(EventHandler<NotificationEventArgs> subscriber)
        {
            lock (_lock)
            {
                if (_subscribers.Contains(subscriber))
                {
                    _subscribers.Remove(subscriber);
                    NotificationEvent -= subscriber;
                    Log($"Subscriber {subscriber.Method.Name} removed.");
                }
                else
                {
                    Log($"Subscriber {subscriber.Method.Name} not found.");
                }
            }
        }

        /// <summary>
        /// Publishes a notification with a specified message.
        /// </summary>
        /// <param name="message">The message of the notification.</param>
        public void Publish(string message)
        {
            NotificationEventArgs args = new(message);
            OnNotification(args);
        }

        /// <summary>
        /// Raises the NotificationEvent event.
        /// </summary>
        /// <param name="e">The instance containing the event data.</param>
        protected virtual void OnNotification(NotificationEventArgs e)
        {
            EventHandler<NotificationEventArgs>? handler;
            lock (_lock)
            {
                handler = NotificationEvent;
            }

            // Ensure there are subscribers
            if (handler != null)
            {
                foreach (EventHandler<NotificationEventArgs> subscriber in handler.GetInvocationList())
                {
                    try
                    {
                        subscriber(this, e);
                        Log($"Notification sent to {subscriber.Method.Name}.");
                    }
                    catch (Exception ex)
                    {
                        Log($"Error notifying subscriber {subscriber.Method.Name}: {ex.Message}");
                    }
                }
            }
            else
            {
                Log("No subscribers to notify.");
            }
        }

        /// <summary>
        /// Logs a message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        private static void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now}: {message}");
        }
    }

}
