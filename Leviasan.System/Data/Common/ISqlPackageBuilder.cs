namespace System.Data.Common
{
    /// <summary>
    /// Represents an SQL package builder.
    /// </summary>
    public interface ISqlPackageBuilder
    {
        /// <summary>
        /// Applies configuration that is defined in an <see cref="ISqlStatementDirector{TCollection, TReader, TBuilder, TRequest, TResponse}"/> instance.
        /// </summary>
        /// <param name="configuration">The configuration for an SQL statement.</param>
        /// <typeparam name="TCollection">Collects all parameters relevant to a Command object.</typeparam>
        /// <typeparam name="TReader">Provides a means of reading one or more forward-only streams of result sets obtained by executing a command at a data source.</typeparam>
        /// <typeparam name="TBuilder">The type of SQL statement builder.</typeparam>
        /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
        /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
        ISqlPackageBuilder ApplyConfiguration<TCollection, TReader, TBuilder, TRequest, TResponse>(ISqlStatementDirector<TCollection, TReader, TBuilder, TRequest, TResponse> configuration)
            where TCollection : DbParameterCollection
            where TReader : DbDataReader
            where TBuilder : ISqlStatementBuilder<TCollection, TReader, TRequest, TResponse>, new();
    }
}
