namespace System.Data.Common
{
    /// <summary>
    /// Allows configuration for an SQL statement to be factored into a separate class.
    /// Implement this interface, applying configuration for the SQL statement in the <see cref="Configure"/> method,
    /// and then apply the configuration using <see cref="ISqlPackageBuilder.ApplyConfiguration{TCollection, TReader, TBuilder, TRequest, TResponse}(ISqlStatementDirector{TCollection, TReader, TBuilder, TRequest, TResponse})"/>.
    /// </summary>
    /// <typeparam name="TCollection">Collects all parameters relevant to a Command object.</typeparam>
    /// <typeparam name="TReader">Provides a means of reading one or more forward-only streams of result sets obtained by executing a command at a data source.</typeparam>
    /// <typeparam name="TBuilder">The type of SQL statement builder.</typeparam>
    /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
    /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
    public interface ISqlStatementDirector<TCollection, TReader, TBuilder, TRequest, TResponse>
        where TBuilder : ISqlStatementBuilder<TCollection, TReader, TRequest, TResponse>, new()
        where TCollection : DbParameterCollection
        where TReader : DbDataReader
    {
        /// <summary>
        /// The unique key.
        /// </summary>
        string Key { get; }
        /// <summary>
        /// Configures an SQL statement.
        /// </summary>
        void Configure(TBuilder builder);
    }
}
