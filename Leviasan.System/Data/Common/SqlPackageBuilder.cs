using System.Collections.Generic;

namespace System.Data.Common
{
    /// <summary>
    /// Represents an SQL package builder.
    /// </summary>
    internal sealed class SqlPackageBuilder : ISqlPackageBuilder
    {
        /// <summary>
        /// The service collection.
        /// </summary>
        private readonly IDictionary<Type, Type> _services;

        /// <summary>
        /// Initializes a new <see cref="SqlPackageBuilder"/> with a specified service collection in which registered SQL statement directors will be placed.
        /// </summary>
        /// <param name="statements">The service collection in which contains registered SQL statement directors.</param>
        /// <exception cref="ArgumentNullException">The service collection is null.</exception>
        public SqlPackageBuilder(IDictionary<Type, Type> statements)
        {
            _services = statements ?? throw new ArgumentNullException(nameof(statements));
        }

        /// <summary>
        /// Applies configuration that is defined in an <see cref="ISqlStatementDirector{TCollection, TReader, TRequest, TResponse}"/>.
        /// </summary>
        /// <param name="serviceType">The type that implement the <see cref="ISqlStatementDirector{TCollection, TReader, TRequest, TResponse}"/> interface.</param>
        /// <exception cref="InvalidCastException">The specified type does not implement the interface <see cref="ISqlStatementDirector{TCollection, TReader, TRequest, TResponse}"/>.</exception>
        public ISqlPackageBuilder ApplyConfiguration(Type serviceType)
        {
            if (serviceType.GetInterface(typeof(ISqlStatementDirector<,,,>).Name) == null)
                throw new InvalidCastException(string.Format(null, Properties.Resources.DoesNotImplementInterface, serviceType, typeof(ISqlStatementDirector<,,,>).Name));

            _services.Add(serviceType, serviceType);
            return this;
        }
        /// <summary>
        /// Applies configuration that is defined in an <see cref="ISqlStatementDirector{TCollection, TReader, TRequest, TResponse}"/>.
        /// </summary>
        /// <typeparam name="TDirector">The type that implement the <see cref="ISqlStatementDirector{TCollection, TReader, TRequest, TResponse}"/> interface.</typeparam>
        /// <exception cref="InvalidCastException">The specified type does not implement the interface <see cref="ISqlStatementDirector{TCollection, TReader, TRequest, TResponse}"/>.</exception>
        public ISqlPackageBuilder ApplyConfiguration<TDirector>() where TDirector : new()
        {
            return ApplyConfiguration(typeof(TDirector));
        }
    }
}
