using System.Collections.Generic;

namespace System.Data
{
    /// <summary>
    /// Provides data for <see cref="Common.ISqlPackage{TCollection, TReader}.Executing"/> event.
    /// </summary>
    public sealed class DbCommandEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance <see cref="DbCommandEventArgs"/> class.
        /// </summary>
        /// <param name="commandText">Command string relevant to <see cref="IDbCommand"/>.</param>
        /// <param name="parameters">Collection of parameters relevant to <see cref="IDbCommand"/>.</param>
        public DbCommandEventArgs(string commandText, IDictionary<string, string> parameters = null)
        {
            CommandText = commandText ?? throw new ArgumentNullException(nameof(commandText));
            Parameters = parameters ?? new Dictionary<string, string>();
        }
        /// <summary>
        /// Initializes a new instance <see cref="DbCommandEventArgs"/> class.
        /// </summary>
        public DbCommandEventArgs(IDbCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            CommandText = command.CommandText;
            Parameters = new Dictionary<string, string>();
            foreach (IDataParameter parameter in command.Parameters)
                Parameters.Add(parameter.ParameterName, parameter.Value?.ToString());
        }

        /// <summary>
        /// Command string.
        /// </summary>
        public string CommandText { get; }
        /// <summary>
        /// Collection of parameters relevant to <see cref="IDbCommand"/>.
        /// </summary>
        public IDictionary<string, string> Parameters { get; }
    }
}
