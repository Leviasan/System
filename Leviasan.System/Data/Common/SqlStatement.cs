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
    public class SqlStatement<TCollection, TReader, TRequest, TResponse> : ISqlStatement<TCollection, TReader, TRequest, TResponse>
        where TCollection : DbParameterCollection
        where TReader : DbDataReader
    {
        /// <summary>
        /// Command string.
        /// </summary>
        public string CommandText { get; set; }
        /// <summary>
        /// Specifies how a command string is interpreted.
        /// </summary>
        public CommandType CommandType { get; set; }
        /// <summary>
        /// The delegate to add command parameters.
        /// </summary>
        public Action<TCollection, TRequest> Parameters { get; set; }
        /// <summary>
        /// The delegate to reading the data from the result.
        /// </summary>
        public Func<TReader, TResponse> Reader { get; set; }
        /// <summary>
        /// The delegate to asynchronously reading the data from the result.
        /// </summary>
        public Func<TReader, CancellationToken, Task<TResponse>> ReaderAsync { get; set; }

        /// <summary>
        /// Creates, configured and returns a <see cref="DbCommand"/> object associated with the specified connection.
        /// </summary>
        /// <param name="connection">The specified connection.</param>
        /// <param name="request">The request data.</param>
        [Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Using a parameterized command string")]
        public virtual DbCommand CreateDbCommand(DbConnection connection, TRequest request)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (request != null && Parameters == null)
                throw new InvalidOperationException(string.Format(null, Properties.Resources.SqlStatementInvalidState, nameof(Parameters)));

            // Open connection
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            // Create and set command property
            var command = connection.CreateCommand();
            command.CommandText = CommandText;
            command.CommandType = CommandType;
            // Add parameters
            if (request != null)
                Parameters?.Invoke(command.Parameters as TCollection, request);

            return command;
        }
    }
}
