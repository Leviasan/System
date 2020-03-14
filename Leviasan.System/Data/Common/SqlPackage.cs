using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Common
{
    /// <summary>
    /// Represents an SQL package.
    /// </summary>
    /// <typeparam name="TCollection">Collects all parameters relevant to a Command object.</typeparam>
    /// <typeparam name="TReader">Provides a means of reading one or more forward-only streams of result sets obtained by executing a command at a data source.</typeparam>
    public abstract class SqlPackage<TCollection, TReader> : ISqlPackage<TCollection, TReader>
        where TCollection : DbParameterCollection
        where TReader : DbDataReader
    {
        /// <summary>
        /// To detect redundant calls <see cref="Dispose()"/> method.
        /// </summary>
        private bool _disposedValue;
        /// <summary>
        /// Dictionary in which contains registered SQL statements.
        /// </summary>
        private readonly IDictionary<string, object> _statements;
        /// <summary>
        /// The event reports that registration of SQL statements has begun.
        /// </summary>
        private event Action<ISqlPackageBuilder> InitializeSqlStatements;

        /// <summary>
        /// Initializes a new  instance <see cref="SqlPackage{TCollection, TReader}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">The connection in null.</exception>
        public SqlPackage(DbConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));

            _statements = new Dictionary<string, object>();
            InitializeSqlStatements += OnInitializeSqlStatements;
            InitializeSqlStatements.Invoke(new SqlPackageBuilder(_statements));
        }

        /// <summary>
        /// The database connection.
        /// </summary>
        public virtual DbConnection Connection { get; }

        /// <summary>
        /// Releases the unmanaged resources and releases the managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Executes SQL statements.
        /// </summary>
        /// <param name="key">The key of the SQL statement.</param>
        public int Execute(string key)
        {
            return Execute<object>(key, null);
        }
        /// <summary>
        /// Executes SQL statements.
        /// </summary>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <param name="key">The key of the SQL statement.</param>
        /// <param name="request">The request data.</param>
        public int Execute<TRequest>(string key, TRequest request)
        {
            // Get SQL statement
            var statement = GetSqlStatement<TRequest, object>(key);
            // Open connection
            OpenConnection();
            // Create and configured command
            using var command = statement.CreateDbCommand(Connection, request);
            // Execute
            return command.ExecuteNonQuery();
        }
        /// <summary>
        /// Asynchronously executes SQL statements.
        /// </summary>
        /// <param name="key">The key of the SQL statement.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        public Task<int> ExecuteAsync(string key, CancellationToken cancellationToken = default)
        {
            return ExecuteAsync<object>(key, null, cancellationToken);
        }
        /// <summary>
        /// Asynchronously executes SQL statements.
        /// </summary>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <param name="key">The key of the SQL statement.</param>
        /// <param name="request">The request data.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        public async Task<int> ExecuteAsync<TRequest>(string key, TRequest request, CancellationToken cancellationToken = default)
        {
            // Get SQL statement
            var statement = GetSqlStatement<TRequest, object>(key);
            // Open connection
            await OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
            // Create and configured command
            using var command = statement.CreateDbCommand(Connection, request);
            // Execute
            return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Executes SQL statements and reading the result data.
        /// </summary>
        /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
        /// <param name="key">The key of the SQL statement.</param>
        public TResponse ExecuteReader<TResponse>(string key)
        {
            return ExecuteReader<object, TResponse>(key, null);
        }
        /// <summary>
        /// Executes SQL statements and reading the result data.
        /// </summary>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
        /// <param name="key">The key of the SQL statement.</param>
        /// <param name="request">The request data.</param>
        public TResponse ExecuteReader<TRequest, TResponse>(string key, TRequest request)
        {
            // Get SQL statement
            var statement = GetSqlStatement<TRequest, TResponse>(key);
            // Checks the SQL statement is fully configured
            if (statement.Reader == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.SqlStatementInvalidState, nameof(statement.Reader)));

            // Open connection
            OpenConnection();
            // Response value
            TResponse response = default;
            // Create and configured command
            using (var command = statement.CreateDbCommand(Connection, request))
            {
                // Execute
                using (var reader = command.ExecuteReader())
                {
                    response = statement.Reader(reader as TReader);
                };
            }
            return response;
        }
        /// <summary>
        /// Asynchronously executes SQL statements and reading the result data.
        /// </summary>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
        /// <param name="key">The key of the SQL statement.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        public Task<TResponse> ExecuteReaderAsync<TRequest, TResponse>(string key, CancellationToken cancellationToken = default)
        {
            return ExecuteReaderAsync<object, TResponse>(key, null, cancellationToken);
        }
        /// <summary>
        /// Asynchronously executes SQL statements and reading the result data.
        /// </summary>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
        /// <param name="key">The key of the SQL statement.</param>
        /// <param name="request">The request data.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        public async Task<TResponse> ExecuteReaderAsync<TRequest, TResponse>(string key, TRequest request, CancellationToken cancellationToken = default)
        {
            // Get SQL statement
            var statement = GetSqlStatement<TRequest, TResponse>(key);
            // Checks the SQL statement is fully configured
            if (statement.ReaderAsync == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.SqlStatementInvalidState, nameof(statement.ReaderAsync)));

            // Open connection
            await OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
            // Response value
            TResponse response = default;
            // Create and configured command
            using (var command = statement.CreateDbCommand(Connection, request))
            {
                // Execute
                using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
                {
                    response = await statement.ReaderAsync(reader as TReader, cancellationToken).ConfigureAwait(false);
                };
            }
            return response;
        }

        /// <summary>
        /// Releases the unmanaged resources and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Connection.Dispose();
                }
                _disposedValue = true;
            }
        }
        /// <summary>
        /// The registration of <see cref="ISqlStatement{TCollection, TReader, TRequest, TResponse}"/>.
        /// </summary>
        /// <param name="builder">The SQL package builder.</param>
        protected abstract void OnInitializeSqlStatements(ISqlPackageBuilder builder);

        /// <summary>
        /// Gets the SQL statement.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
        /// <exception cref="KeyNotFoundException">The key is not found in dictionary which contains collects of SQL statements.</exception>
        private ISqlStatement<TCollection, TReader, TRequest, TResponse> GetSqlStatement<TRequest, TResponse>(string key)
        {
            if (!(_statements[key] is ISqlStatement<TCollection, TReader, TRequest, TResponse> statement))
                throw new KeyNotFoundException();

            return statement;
        }
        /// <summary>
        /// Opens connection to the database if it closed.
        /// </summary>
        private void OpenConnection()
        {
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();
        }
        /// <summary>
        /// Asynchronously opens connection to the database if it closed.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        private async Task OpenConnectionAsync(CancellationToken cancellationToken)
        {
            if (Connection.State == ConnectionState.Closed)
                await Connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
