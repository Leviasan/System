namespace System.Data.Common
{
    /// <summary>
    /// Represents an SQL package builder.
    /// </summary>
    public interface ISqlPackageBuilder
    {
        /// <summary>
        /// Applies configuration that is defined in an <see cref="ISqlStatementDirector{TCollection, TReader, TRequest, TResponse}"/>.
        /// </summary>
        /// <param name="serviceType">The type that implement the <see cref="ISqlStatementDirector{TCollection, TReader, TRequest, TResponse}"/> interface.</param>
        ISqlPackageBuilder ApplyConfiguration(Type serviceType);
        /// <summary>
        /// Applies configuration that is defined in an <see cref="ISqlStatementDirector{TCollection, TReader, TRequest, TResponse}"/>.
        /// </summary>
        /// <typeparam name="TDirector">The type that implement the <see cref="ISqlStatementDirector{TCollection, TReader, TRequest, TResponse}"/> interface.</typeparam>
        ISqlPackageBuilder ApplyConfiguration<TDirector>() where TDirector : new();
    }
}
