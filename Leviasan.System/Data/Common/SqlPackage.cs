using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace System.Data.Common
{
    /// <summary>
    /// Represents an SQL package that contains <see cref="ISqlStatementDirector{TCollection, TReader, TRequest, TResponse}"/> and can execute them.
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
        /// The service provider.
        /// </summary>
        private readonly IServiceProvider _provider;
        /// <summary>
        /// The event reports that the registration of SQL statement directors has begun.
        /// </summary>
        private event Action<ISqlPackageBuilder> InitializeSqlStatementDirectors;

        /// <summary>
        /// Initializes a new  instance <see cref="SqlPackage{TCollection, TReader}"/> class.
        /// </summary>
        protected SqlPackage()
        {
            var services = new ServiceCollection();
            InitializeSqlStatementDirectors += OnInitialize;
            InitializeSqlStatementDirectors.Invoke(new SqlPackageBuilder(services));
            var options = new ServiceProviderOptions() { ValidateScopes = true, ValidateOnBuild = true };
            var builder = new DefaultServiceProviderFactory(options);
            _provider = builder.CreateServiceProvider(services);
        }

        /// <summary>
        /// Gets the database connection.
        /// </summary>
        public abstract DbConnection Connection { get; }

        /// <summary>
        /// Dispose database connection.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Executes a SQL statement against a connection object.
        /// </summary>
        /// <typeparam name="TDirector">The type that represents the configuration of the SQL statement.</typeparam>
        /// <returns>The number of rows affected.</returns>
        public int Execute<TDirector>()
            where TDirector : ISqlStatementDirector<TCollection, TReader, object, object>
        {
            return Execute<TDirector, object>(null);
        }
        /// <summary>
        /// Executes a SQL statement against a connection object.
        /// </summary>
        /// <typeparam name="TDirector">The type that represents the configuration of the SQL statement.</typeparam>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <returns>The number of rows affected.</returns>
        public int Execute<TDirector, TRequest>(TRequest request)
            where TDirector : ISqlStatementDirector<TCollection, TReader, TRequest, object>
        {
            // Get SQL statement
            var statement = GetSqlStatement<TRequest, object>(typeof(TDirector));
            // Create and configured command
            using var command = statement.CreateDbCommand(Connection, request);
            // Execute
            return command.ExecuteNonQuery();
        }
        /// <summary>
        /// Asynchronously executes a SQL statement against a connection object.
        /// </summary>
        /// <typeparam name="TDirector">The type that represents the configuration of the SQL statement.</typeparam>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        public Task<int> ExecuteAsync<TDirector>(CancellationToken cancellationToken)
            where TDirector : ISqlStatementDirector<TCollection, TReader, object, object>
        {
            return ExecuteAsync<TDirector, object>(null, cancellationToken);
        }
        /// <summary>
        /// Asynchronously executes a SQL statement against a connection object.
        /// </summary>
        /// <typeparam name="TDirector">The type that represents the configuration of the SQL statement.</typeparam>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <param name="request">The request data.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        public async Task<int> ExecuteAsync<TDirector, TRequest>(TRequest request, CancellationToken cancellationToken)
            where TDirector : ISqlStatementDirector<TCollection, TReader, TRequest, object>
        {
            // Get SQL statement
            var statement = GetSqlStatement<TRequest, object>(typeof(TDirector));
            // Create and configured command
            using var command = statement.CreateDbCommand(Connection, request);
            // Execute
            return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Executes a SQL statement against a connection object and reading the response data.
        /// </summary>
        /// <typeparam name="TDirector">The type that represents the configuration of the SQL statement.</typeparam>
        /// <typeparam name="TResponse">The data type describing the output data.</typeparam>
        public TResponse ExecuteReader<TDirector, TResponse>()
            where TDirector : ISqlStatementDirector<TCollection, TReader, object, TResponse>
        {
            return ExecuteReader<TDirector, object, TResponse>(null);
        }
        /// <summary>
        /// Executes a SQL statement against a connection object and reading the response data.
        /// </summary>
        /// <typeparam name="TDirector">The type that represents the configuration of the SQL statement.</typeparam>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <typeparam name="TResponse">The data type describing the output data.</typeparam>
        /// <param name="request">The request data.</param>
        public TResponse ExecuteReader<TDirector, TRequest, TResponse>(TRequest request)
            where TDirector : ISqlStatementDirector<TCollection, TReader, TRequest, TResponse>
        {
            // Get SQL statement
            var statement = GetSqlStatement<TRequest, TResponse>(typeof(TDirector));
            // Checks the SQL statement is fully configured
            if (statement.Reader == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.SqlStatementInvalidState, nameof(statement.Reader)));
            // Response value
            TResponse response = default;
            // Create and configured command
            using (var command = statement.CreateDbCommand(Connection, request))
            {
                // Execute reader
                using (var reader = command.ExecuteReader())
                {
                    response = statement.Reader.Invoke(reader as TReader);
                };
            }
            return response;
        }
        /// <summary>
        /// Asynchronously executes a SQL statement against a connection object and reading the response data.
        /// </summary>
        /// <typeparam name="TDirector">The type that represents the configuration of the SQL statement.</typeparam>
        /// <typeparam name="TResponse">The data type describing the output data.</typeparam>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        public Task<TResponse> ExecuteReaderAsync<TDirector, TResponse>(CancellationToken cancellationToken)
            where TDirector : ISqlStatementDirector<TCollection, TReader, object, TResponse>
        {
            return ExecuteReaderAsync<TDirector, object, TResponse>(null, cancellationToken);
        }
        /// <summary>
        /// Asynchronously executes a SQL statement against a connection object and reading the response data.
        /// </summary>
        /// <typeparam name="TDirector">The type that represents the configuration of the SQL statement.</typeparam>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <typeparam name="TResponse">The data type describing the output data.</typeparam>
        /// <param name="request">The request data.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        public async Task<TResponse> ExecuteReaderAsync<TDirector, TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TDirector : ISqlStatementDirector<TCollection, TReader, TRequest, TResponse>
        {
            // Get SQL statement
            var statement = GetSqlStatement<TRequest, TResponse>(typeof(TDirector));
            // Checks the SQL statement is fully configured
            if (statement.ReaderAsync == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.SqlStatementInvalidState, nameof(statement.ReaderAsync)));
            // Response value
            TResponse response = default;
            // Create and configured command
            using (var command = statement.CreateDbCommand(Connection, request))
            {
                // Execute reader
                using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
                {
                    response = await statement.ReaderAsync.Invoke(reader as TReader, cancellationToken).ConfigureAwait(false);
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
                    Connection?.Dispose();
                    ((IDisposable)_provider).Dispose();
                }
                _disposedValue = true;
            }
        }
        /// <summary>
        /// The registration of <see cref="ISqlStatementDirector{TCollection, TReader, TRequest, TResponse}"/>.
        /// </summary>
        /// <param name="builder">The SQL package builder.</param>
        protected abstract void OnInitialize(ISqlPackageBuilder builder);

        /// <summary>
        /// Gets the SQL statement.
        /// </summary>
        /// <param name="serviceType">The type.</param>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <typeparam name="TResponse">The data type describing the output data.</typeparam>
        /// <exception cref="InvalidOperationException">There is no service of type serviceType.</exception>
        private ISqlStatement<TCollection, TReader, TRequest, TResponse> GetSqlStatement<TRequest, TResponse>(Type serviceType)
        {
            var director = _provider.GetRequiredService(serviceType) as ISqlStatementDirector<TCollection, TReader, TRequest, TResponse>;
            var builder = director.CreateBuilder();
            director.Configure(builder);
            var statement = builder.CreateInstance();
            return statement;
        }
    }
}
