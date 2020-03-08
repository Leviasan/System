using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Common
{
    /// <summary>
    /// Represents an SQL statement that is executed while connected to a data source.
    /// </summary>
    /// <typeparam name="TCollection">Collects all parameters relevant to a Command object.</typeparam>
    /// <typeparam name="TReader">Provides a means of reading one or more forward-only streams of result sets obtained by executing a command at a data source.</typeparam>
    /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
    /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
    public interface ISqlStatement<TCollection, TReader, TRequest, TResponse>
        where TCollection : DbParameterCollection
        where TReader : DbDataReader
    {
        /// <summary>
        /// Command string.
        /// </summary>
        string CommandText { get; }
        /// <summary>
        /// Specifies how a command string is interpreted.
        /// </summary>
        CommandType CommandType { get; }
        /// <summary>
        /// The delegate to add command parameters.
        /// </summary>
        Action<TCollection, TRequest> Parameters { get; }
        /// <summary>
        /// The delegate to reading the data from the result.
        /// </summary>
        Func<TReader, TResponse> Reader { get; }
        /// <summary>
        /// The delegate to asynchronously reading the data from the result.
        /// </summary>
        Func<TReader, CancellationToken, Task<TResponse>> ReaderAsync { get; }

        /// <summary>
        /// Creates, configured and returns a <see cref="DbCommand"/> object associated with the specified connection.
        /// </summary>
        /// <param name="connection">The specified connection.</param>
        /// <param name="request">The request data.</param>
        DbCommand CreateDbCommand(DbConnection connection, TRequest request);
    }
}
