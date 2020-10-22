using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;

namespace Lightwind.DbConnection
{
    public static class DbConnectionFactoryExtensions
    {
        public static void AddDbConnectionFactoryBuilder(this IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionFactoryBuilder, DbConnectionFactoryBuilder>();
        }

        public static void AddSingletonDbConnectionFactory(this IServiceCollection services, string connName)
        {
            services.AddDbConnectionFactoryBuilder();
            services.AddSingleton((sp) =>
            {
                var maker = sp.GetRequiredService<IDbConnectionFactoryBuilder>();
                return maker.CreateDbConnectionFactory(connName);
            });
        }
    }

    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    internal class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connString;

        public DbConnectionFactory(string connString)
        {
            if (string.IsNullOrWhiteSpace(connString))
            {
                throw new ArgumentException("Connection string can't be empty!");
            }
            _connString = connString;
        }

        public IDbConnection CreateConnection()
        {
            IDbConnection connection = new SqlConnection(_connString);
            return connection;
        }
    }
    public interface IDbConnectionFactoryBuilder
    {
        IDbConnectionFactory CreateDbConnectionFactory(string connName);
    }

    internal class DbConnectionFactoryBuilder : IDbConnectionFactoryBuilder
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactoryBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnectionFactory CreateDbConnectionFactory(string connName)
        {
            var strConn = GetConnectionString(connName);
            return new DbConnectionFactory(strConn);
        }

        string GetConnectionString(string connName)
        {
            if (string.IsNullOrWhiteSpace(connName))
            {
                throw new ArgumentException("Connection name is empty.");
            }
            else
            {
                string conn = _configuration.GetConnectionString(connName);
                if (string.IsNullOrWhiteSpace(conn))
                {
                    throw new ArgumentException("Can't get connection string by connection name, check appsettings.json.");
                }
                return conn;
            }
        }
    }
}
