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
        /// To detect redundant calls <see cref="Dispose"/> method.
        /// </summary>
        private bool _disposedValue = false;
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

        public event EventHandler<DbCommandEventArgs> Executing;

        public virtual DbConnection Connection { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public int Execute(string key)
        {
            return Execute<object>(key, null);
        }
        public int Execute<TRequest>(string key, TRequest request)
        {
            // Get SQL statement
            var statement = GetSqlStatement<TRequest, object>(key);

            // Open connection
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();

            // Create and configured command
            using var command = statement.CreateDbCommand(Connection, request);
            // Notify before executing command
            Executing?.Invoke(this, new DbCommandEventArgs(command));
            // Execute
            return command.ExecuteNonQuery();
        }
        public Task<int> ExecuteAsync(string key, CancellationToken cancellationToken = default)
        {
            return ExecuteAsync<object>(key, null, cancellationToken);
        }
        public async Task<int> ExecuteAsync<TRequest>(string key, TRequest request, CancellationToken cancellationToken = default)
        {
            // Get SQL statement
            var statement = GetSqlStatement<TRequest, object>(key);

            // Open connection
            if (Connection.State == ConnectionState.Closed)
                await Connection.OpenAsync(cancellationToken).ConfigureAwait(false);

            // Create and configured command
            using var command = statement.CreateDbCommand(Connection, request);
            // Notify before executing command
            Executing?.Invoke(this, new DbCommandEventArgs(command));
            // Execute
            return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
        public TResponse ExecuteReader<TResponse>(string key)
        {
            return ExecuteReader<object, TResponse>(key, null);
        }
        public TResponse ExecuteReader<TRequest, TResponse>(string key, TRequest request)
        {
            // Get SQL statement
            var statement = GetSqlStatement<TRequest, TResponse>(key);
            // Checks the SQL statement is fully configured
            if (statement.Reader == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.SqlStatementInvalidState, nameof(statement.Reader)));

            // Open connection
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();

            // Response value
            TResponse response = default;
            // Create and configured command
            using (var command = statement.CreateDbCommand(Connection, request))
            {
                // Notify before executing command
                Executing?.Invoke(this, new DbCommandEventArgs(command));
                // Execute
                using (var reader = command.ExecuteReader())
                {
                    response = statement.Reader(reader as TReader);
                };
            }
            return response;
        }
        public Task<TResponse> ExecuteReaderAsync<TRequest, TResponse>(string key, CancellationToken cancellationToken = default)
        {
            return ExecuteReaderAsync<object, TResponse>(key, null, cancellationToken);
        }
        public async Task<TResponse> ExecuteReaderAsync<TRequest, TResponse>(string key, TRequest request, CancellationToken cancellationToken = default)
        {
            // Get SQL statement
            var statement = GetSqlStatement<TRequest, TResponse>(key);
            // Checks the SQL statement is fully configured
            if (statement.ReaderAsync == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.SqlStatementInvalidState, nameof(statement.ReaderAsync)));

            // Open connection
            if (Connection.State == ConnectionState.Closed)
                await Connection.OpenAsync(cancellationToken).ConfigureAwait(false);

            // Response value
            TResponse response = default;
            // Create and configured command
            using (var command = statement.CreateDbCommand(Connection, request))
            {
                // Notify before executing command
                Executing?.Invoke(this, new DbCommandEventArgs(command));
                // Execute
                using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
                {
                    response = await statement.ReaderAsync(reader as TReader, cancellationToken).ConfigureAwait(false);
                };
            }
            return response;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">This indicates whether the method call comes from a <see cref="Dispose"/> method (its value is true) or from a finalizer (its value is false).</param>
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
        /// The registration of <see cref="ISqlStatement{TRequest, TResponse}"/>.
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
    }
}
