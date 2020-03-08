using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Common
{
    /// <summary>
    /// Represents an SQL package.
    /// </summary>
    /// <typeparam name="TCollection">Collects all parameters relevant to a Command object.</typeparam>
    /// <typeparam name="TReader">Provides a means of reading one or more forward-only streams of result sets obtained by executing a command at a data source.</typeparam>
    public interface ISqlPackage<TCollection, TReader> : IDisposable
        where TCollection : DbParameterCollection
        where TReader : DbDataReader
    {
        /// <summary>
        /// Notification before calling <see cref="IDbCommand"/>.
        /// </summary>
        event EventHandler<DbCommandEventArgs> Executing;
        /// <summary>
        /// The database connection.
        /// </summary>
        DbConnection Connection { get; }

        /// <summary>
        /// Executes SQL statements.
        /// </summary>
        /// <param name="key">The key of the SQL statement in the package.</param>
        int Execute(string key);
        /// <summary>
        /// Executes SQL statements.
        /// </summary>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <param name="key">The key of the SQL statement in the package.</param>
        /// <param name="request">The request data.</param>
        int Execute<TRequest>(string key, TRequest request);
        /// <summary>
        /// Asynchronously executes SQL statements.
        /// </summary>
        /// <param name="key">The key of the SQL statement in the package.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        Task<int> ExecuteAsync(string key, CancellationToken cancellationToken);
        /// <summary>
        /// Asynchronously executes SQL statements.
        /// </summary>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <param name="key">The key of the SQL statement in the package.</param>
        /// <param name="request">The request data.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        Task<int> ExecuteAsync<TRequest>(string key, TRequest request, CancellationToken cancellationToken);
        /// <summary>
        /// Executes SQL statements and reading the result data.
        /// </summary>
        /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
        /// <param name="key">The key of the SQL statement in the package.</param>
        TResponse ExecuteReader<TResponse>(string key);
        /// <summary>
        /// Executes SQL statements and reading the result data.
        /// </summary>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
        /// <param name="key">The key of the SQL statement in the package.</param>
        /// <param name="request">The request data.</param>
        TResponse ExecuteReader<TRequest, TResponse>(string key, TRequest request);
        /// <summary>
        /// Asynchronously executes SQL statements and reading the result data.
        /// </summary>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
        /// <param name="key">The key of the SQL statement in the package.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        Task<TResponse> ExecuteReaderAsync<TRequest, TResponse>(string key, CancellationToken cancellationToken);
        /// <summary>
        /// Asynchronously executes SQL statements and reading the result data.
        /// </summary>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
        /// <param name="key">The key of the SQL statement in the package.</param>
        /// <param name="request">The request data.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        Task<TResponse> ExecuteReaderAsync<TRequest, TResponse>(string key, TRequest request, CancellationToken cancellationToken);
    }
}
