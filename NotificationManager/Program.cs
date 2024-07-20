namespace NotificationManager
{
    public class Program
    {
        static void Main()
        {
            FacebookPublisher publisher = new();

            // Adding subscribers
            publisher.AddSubscriber(User);
            publisher.AddSubscriber(User);
            publisher.AddSubscriber(Admin);

            // Publish a notification
            publisher.Publish("Hello, this is a user notification!");

            // Removing a subscriber
            publisher.RemoveSubscriber(User);
            publisher.RemoveSubscriber(User);

            // Try to publish another notification
            publisher.Publish("This should trigger only the admin notification!");

            // Removing the remaining subscriber
            publisher.RemoveSubscriber(Admin);

            // Try to publish another notification with no subscribers
            publisher.Publish("This should not trigger any notification!");

            Console.ReadLine();
        }

        /// <summary>
        /// Handles user notifications.
        /// </summary>
        private static void User(object? sender, NotificationEventArgs e)
        {
            Console.WriteLine($"User notification received: {e.Message} at {e.Timestamp}");
        }

        /// <summary>
        /// Handles admin notifications.
        /// </summary>
        private static void Admin(object? sender, NotificationEventArgs e)
        {
            Console.WriteLine($"Admin notification received: {e.Message} at {e.Timestamp}");
        }
    }
}
