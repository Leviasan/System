using System.Collections.Generic;

namespace System.Data.Common
{
    /// <summary>
    /// Represents an SQL package builder.
    /// </summary>
    internal sealed class SqlPackageBuilder : ISqlPackageBuilder
    {
        /// <summary>
        /// Dictionary in which contains registered SQL statements.
        /// </summary>
        private readonly IDictionary<string, object> _statements;

        /// <summary>
        /// Initializes a new <see cref="SqlPackageBuilder"/> with a specified dictionary in which registered SQL statements will be placed.
        /// </summary>
        /// <param name="statements">Dictionary in which contains registered SQL statements.</param>
        /// <exception cref="ArgumentNullException">Dictionary is null.</exception>
        public SqlPackageBuilder(IDictionary<string, object> statements)
        {
            _statements = statements ?? throw new ArgumentNullException(nameof(statements));
        }

        /// <summary>
        /// Applies configuration that is defined in an <see cref="ISqlStatementDirector{TCollection, TReader, TBuilder, TRequest, TResponse}"/> instance.
        /// </summary>
        /// <param name="configuration">The configuration for an SQL statement.</param>
        /// <typeparam name="TCollection">Collects all parameters relevant to a Command object.</typeparam>
        /// <typeparam name="TReader">Provides a means of reading one or more forward-only streams of result sets obtained by executing a command at a data source.</typeparam>
        /// <typeparam name="TBuilder">The type of SQL statement builder.</typeparam>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
        /// <exception cref="ArgumentNullException">The configuration is null.</exception>
        /// <exception cref="KeyNotFoundException">The key of SQL statement configuration is null, empty, or consists only of white-space characters.</exception>
        public ISqlPackageBuilder ApplyConfiguration<TCollection, TReader, TBuilder, TRequest, TResponse>(ISqlStatementDirector<TCollection, TReader, TBuilder, TRequest, TResponse> configuration)
            where TCollection : DbParameterCollection
            where TReader : DbDataReader
            where TBuilder : ISqlStatementBuilder<TCollection, TReader, TRequest, TResponse>, new()
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(configuration.Key))
                throw new KeyNotFoundException(Properties.Resources.StringIsMissing);

            var builder = Activator.CreateInstance<TBuilder>();
            configuration.Configure(builder);
            var statement = builder.Build();
            _statements.Add(configuration.Key, statement);

            return this;
        }
    }
}
