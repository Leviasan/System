using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Common
{
    /// <summary>
    /// Represents an SQL package that contains <see cref="ISqlStatementDirector{TCollection, TReader, TRequest, TResponse}"/> and can execute them.
    /// </summary>
    /// <typeparam name="TCollection">Collects all parameters relevant to a Command object.</typeparam>
    /// <typeparam name="TReader">Provides a means of reading one or more forward-only streams of result sets obtained by executing a command at a data source.</typeparam>
    public interface ISqlPackage<TCollection, TReader> : IDisposable 
        where TCollection : DbParameterCollection 
        where TReader : DbDataReader
    {
        /// <summary>
        /// Gets the database connection.
        /// </summary>
        DbConnection Connection { get; }

        /// <summary>
        /// Executes a SQL statement against a connection object.
        /// </summary>
        /// <typeparam name="TDirector">The type that represents the configuration of the SQL statement.</typeparam>
        /// <returns>The number of rows affected.</returns>
        int Execute<TDirector>() 
            where TDirector : ISqlStatementDirector<TCollection, TReader, object, object>;
        /// <summary>
        /// Executes a SQL statement against a connection object.
        /// </summary>
        /// <typeparam name="TDirector">The type that represents the configuration of the SQL statement.</typeparam>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <param name="request">The request data.</param>
        /// <returns>The number of rows affected.</returns>
        int Execute<TDirector, TRequest>(TRequest request) 
            where TDirector : ISqlStatementDirector<TCollection, TReader, TRequest, object>;
        /// <summary>
        /// Asynchronously executes a SQL statement against a connection object.
        /// </summary>
        /// <typeparam name="TDirector">The type that represents the configuration of the SQL statement.</typeparam>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        Task<int> ExecuteAsync<TDirector>(CancellationToken cancellationToken) 
            where TDirector : ISqlStatementDirector<TCollection, TReader, object, object>;
        /// <summary>
        /// Asynchronously executes a SQL statement against a connection object.
        /// </summary>
        /// <typeparam name="TDirector">The type that represents the configuration of the SQL statement.</typeparam>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <param name="request">The request data.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        Task<int> ExecuteAsync<TDirector, TRequest>(TRequest request, CancellationToken cancellationToken) 
            where TDirector : ISqlStatementDirector<TCollection, TReader, TRequest, object>;
        /// <summary>
        /// Executes a SQL statement against a connection object and reading the response data.
        /// </summary>
        /// <typeparam name="TDirector">The type that represents the configuration of the SQL statement.</typeparam>
        /// <typeparam name="TResponse">The data type describing the output data.</typeparam>
        TResponse ExecuteReader<TDirector, TResponse>() 
            where TDirector : ISqlStatementDirector<TCollection, TReader, object, TResponse>;
        /// <summary>
        /// Executes a SQL statement against a connection object and reading the response data.
        /// </summary>
        /// <typeparam name="TDirector">The type that represents the configuration of the SQL statement.</typeparam>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <typeparam name="TResponse">The data type describing the output data.</typeparam>
        /// <param name="request">The request data.</param>
        TResponse ExecuteReader<TDirector, TRequest, TResponse>(TRequest request) 
            where TDirector : ISqlStatementDirector<TCollection, TReader, TRequest, TResponse>;
        /// <summary>
        /// Asynchronously executes a SQL statement against a connection object and reading the response data.
        /// </summary>
        /// <typeparam name="TDirector">The type that represents the configuration of the SQL statement.</typeparam>
        /// <typeparam name="TResponse">The data type describing the output data.</typeparam>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        Task<TResponse> ExecuteReaderAsync<TDirector, TResponse>(CancellationToken cancellationToken) 
            where TDirector : ISqlStatementDirector<TCollection, TReader, object, TResponse>;
        /// <summary>
        /// Asynchronously executes a SQL statement against a connection object and reading the response data.
        /// </summary>
        /// <typeparam name="TDirector">The type that represents the configuration of the SQL statement.</typeparam>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <typeparam name="TResponse">The data type describing the output data.</typeparam>
        /// <param name="request">The request data.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        Task<TResponse> ExecuteReaderAsync<TDirector, TRequest, TResponse>(TRequest request, CancellationToken cancellationToken) 
            where TDirector : ISqlStatementDirector<TCollection, TReader, TRequest, TResponse>;
    }
}
