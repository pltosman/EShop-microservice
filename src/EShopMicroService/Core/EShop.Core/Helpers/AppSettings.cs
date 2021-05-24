using System;
namespace EShop.Core.Helpers
{
    public class AppSettings
    {
        public TokenSettings TokenSettings { get; set; }
        public RabbitMQSettings RabbitMQSettings { get; set; }

        public PostgresqlSettings PostgresqlSettings { get; set; }
    }

    public class TokenSettings
    {
        public string Secret { get; set; }
        public string JwtTokenIssuer { get; set; }
        public string JwtTokenAudience { get; set; }
        public int JwtTokenLifeTime { get; set; }
    }

    public class RabbitMQSettings
    {
        public string Connection { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RetryCount { get; set; }
        public string SubscriptionClientName { get; set; }
    }

    public class PostgresqlSettings
    {
        public string Host { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public string URI { get; set; }
        public string HerokuCLI { get; set; }
        
    }
}
