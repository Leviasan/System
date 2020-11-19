namespace System.Data.Common
{
    /// <summary>
    /// Allows configuration for an SQL statement to be factored into a separate class.
    /// Implement this interface, applying configuration for the SQL statement in the <see cref="Configure"/> method,
    /// and then apply the configuration using <see cref="ISqlPackageBuilder.ApplyConfiguration(Type)"/> or <see cref="ISqlPackageBuilder.ApplyConfiguration{TDirector}"/>.
    /// </summary>
    /// <typeparam name="TCollection">Collects all parameters relevant to a Command object.</typeparam>
    /// <typeparam name="TReader">Provides a means of reading one or more forward-only streams of result sets obtained by executing a command at a data source.</typeparam>
    /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
    /// <typeparam name="TResponse">The data type describing the output data.</typeparam>
    public interface ISqlStatementDirector<TCollection, TReader, TRequest, TResponse>
        where TCollection : DbParameterCollection
        where TReader : DbDataReader
    {
        /// <summary>
        /// Configures a SQL statement.
        /// </summary>
        /// <param name="builder">The SQL statement builder.</param>
        void Configure(ISqlStatementBuilder<TCollection, TReader, TRequest, TResponse> builder);
        /// <summary>
        /// Creates the SQL statement builder that can create SQL statements.
        /// </summary>
        SqlStatementBuilder<TCollection, TReader, TRequest, TResponse> CreateBuilder();
    }
}
