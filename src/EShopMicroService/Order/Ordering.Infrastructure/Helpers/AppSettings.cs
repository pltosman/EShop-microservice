using System;
namespace Ordering.Infrastructure.Helpers
{
    public class AppSettings
    {
        public RabbitMQSettings RabbitMQSettings { get; set; }

    }

    public class RabbitMQSettings
    {
        public string Connection { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RetryCount { get; set; }
        public string SubscriptionClientName { get; set; }
    }
}
