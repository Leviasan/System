using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Common
{
    /// <summary>
    /// Represents an SQL statement builder.
    /// </summary>
    /// <typeparam name="TCollection">Collects all parameters relevant to a Command object.</typeparam>
    /// <typeparam name="TReader">Provides a means of reading one or more forward-only streams of result sets obtained by executing a command at a data source.</typeparam>
    /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
    /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
    public interface ISqlStatementBuilder<TCollection, TReader, TRequest, TResponse>
        where TCollection : DbParameterCollection
        where TReader : DbDataReader
    {
        /// <summary>
        /// Adds the delegate to add command parameters.
        /// </summary>
        /// <param name="parameters">The delegate to add command parameters.</param>
        ISqlStatementBuilder<TCollection, TReader, TRequest, TResponse> AddParameters(Action<TCollection, TRequest> parameters);
        /// <summary>
        /// Adds the delegate to read the data from the result.
        /// </summary>
        /// <param name="reader">The delegate to reading the data from result.</param>
        ISqlStatementBuilder<TCollection, TReader, TRequest, TResponse> AddReader(Func<TReader, TResponse> reader);
        /// <summary>
        /// Adds the delegate to asynchronously reading the data from the result.
        /// </summary>
        /// <param name="reader">The delegate to asynchronously reading the data from result.</param>
        ISqlStatementBuilder<TCollection, TReader, TRequest, TResponse> AddReaderAsync(Func<TReader, CancellationToken, Task<TResponse>> reader);
        /// <summary>
        /// Returns the SQL statement.
        /// </summary>
        ISqlStatement<TCollection, TReader, TRequest, TResponse> Build();
        /// <summary>
        /// Sets the command string.
        /// </summary>
        ISqlStatementBuilder<TCollection, TReader, TRequest, TResponse> SetCommandText(string commandText);
        /// <summary>
        /// Sets how a command string is interpreted. By default is <see cref="CommandType.Text"/>
        /// </summary>
        /// <param name="commandType">Specifies how a command string is interpreted.</param>
        ISqlStatementBuilder<TCollection, TReader, TRequest, TResponse> SetCommandType(CommandType commandType);
    }
}
