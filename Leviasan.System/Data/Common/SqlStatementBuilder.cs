using System.Collections.Generic;
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
    public abstract class SqlStatementBuilder<TCollection, TReader, TRequest, TResponse> : ISqlStatementBuilder<TCollection, TReader, TRequest, TResponse> 
        where TCollection : DbParameterCollection 
        where TReader : DbDataReader
    {
        /// <summary>
        /// Gets the property value collection of the future SQL statement.
        /// </summary>
        protected IDictionary<string, object> Dictionary { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Adds the delegate to add command parameters.
        /// </summary>
        /// <param name="parameters">The delegate to add command parameters.</param>
        /// <exception cref="ArgumentNullException">Delegate is null.</exception>
        public ISqlStatementBuilder<TCollection, TReader, TRequest, TResponse> AddParameters(Action<TCollection, TRequest> parameters)
        {
            Dictionary.Add(nameof(ISqlStatement<TCollection, TReader, TRequest, object>.Parameters), parameters ?? throw new ArgumentNullException(nameof(parameters)));
            return this;
        }
        /// <summary>
        /// Adds the delegate to reading the data from result.
        /// </summary>
        /// <param name="reader">The delegate to reading the data from result.</param>
        /// <exception cref="ArgumentNullException">Delegate is null.</exception>
        public ISqlStatementBuilder<TCollection, TReader, TRequest, TResponse> AddReader(Func<TReader, TResponse> reader)
        {
            Dictionary.Add(nameof(ISqlStatement<TCollection, TReader, TRequest, TResponse>.Reader), reader ?? throw new ArgumentNullException(nameof(reader)));
            return this;
        }
        /// <summary>
        /// Adds the delegate to asynchronously reading the data from result.
        /// </summary>
        /// <param name="reader">The delegate to asynchronously reading the data from result.</param>
        /// <exception cref="ArgumentNullException">Delegate is null.</exception>
        public ISqlStatementBuilder<TCollection, TReader, TRequest, TResponse> AddReaderAsync(Func<TReader, CancellationToken, Task<TResponse>> reader)
        {
            Dictionary.Add(nameof(ISqlStatement<TCollection, TReader, TRequest, TResponse>.ReaderAsync), reader ?? throw new ArgumentNullException(nameof(reader)));
            return this;
        }
        /// <summary>
        /// Sets the command string.
        /// </summary>
        /// <exception cref="ArgumentException">The specified string can not be null, empty, or consists only of white-space characters.</exception>
        public ISqlStatementBuilder<TCollection, TReader, TRequest, TResponse> SetCommandText(string commandText)
        {
            if (string.IsNullOrWhiteSpace(commandText))
                throw new ArgumentException(Properties.Resources.StringIsMissing, nameof(commandText));

            Dictionary.Add(nameof(ISqlStatement<TCollection, TReader, TRequest, TResponse>.CommandText), commandText);
            return this;
        }
        /// <summary>
        /// Sets how a command string is interpreted. By default is <see cref="CommandType.Text"/>
        /// </summary>
        /// <param name="commandType">Specifies how a command string is interpreted.</param>
        public ISqlStatementBuilder<TCollection, TReader, TRequest, TResponse> SetCommandType(CommandType commandType)
        {
            Dictionary.Add(nameof(ISqlStatement<TCollection, TReader, TRequest, TResponse>.CommandType), commandType);
            return this;
        }
        /// <summary>
        /// Creates the instance of the SQL statement.
        /// </summary>
        public abstract ISqlStatement<TCollection, TReader, TRequest, TResponse> CreateInstance();
    }
}
