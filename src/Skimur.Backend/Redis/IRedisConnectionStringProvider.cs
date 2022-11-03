using System;
namespace Skimur.Backend.Redis
{
	public interface IRedisConnectionStringProvider
	{
        /// <summary>
        /// Is there a valid connection string configured?
        /// </summary>
        bool HasConnectionString { get; }

        /// <summary>
        /// The connection string
        /// </summary>
        string ConnectionString { get; }
    }
}

