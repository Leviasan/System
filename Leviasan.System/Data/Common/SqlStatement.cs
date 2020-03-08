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
        public string CommandText { get; set; }
        public CommandType CommandType { get; set; }
        public Action<TCollection, TRequest> Parameters { get; set; }
        public Func<TReader, TResponse> Reader { get; set; }
        public Func<TReader, CancellationToken, Task<TResponse>> ReaderAsync { get; set; }

        public virtual DbCommand CreateDbCommand(DbConnection connection, TRequest request)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (request != null && Parameters == null)
                throw new InvalidOperationException(string.Format(null, Properties.Resources.SqlStatementInvalidState, nameof(Parameters)));

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
