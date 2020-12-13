using System.Data.SqlTypes;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Common
{
    /// <summary>
    /// The database data reader extensions.
    /// </summary>
    public static class DbDataReaderExtensions
    {
        /// <summary>
        /// Gets the value of the specified column as the requested type.
        /// </summary>
        /// <param name="reader">Database data reader.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultValue">Return value if the column contains non-existent or missing values.</param>
        /// <param name="throwDbNull">Thrown exception if response value is <see cref="DBNull"/>.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <exception cref="ArgumentException">When converting to Enum: return value from the database is either an empty string or only contains white space. -or- Value is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="ArgumentNullException">Reader is null.</exception>
        /// <exception cref="IndexOutOfRangeException">The name specified is not a valid column name.</exception>
        /// <exception cref="InvalidOperationException">The connection drops or is closed during the data retrieval.The System.Data.Common.DbDataReader is closed during the data retrieval.There is no data ready to be read. Tried to read a previously-read column in sequential mode. There was an asynchronous operation in progress. This applies to all Get* methods when running in sequential mode, as they could be called while reading a stream.</exception>
        /// <exception cref="OverflowException">When converting to Enum: value is outside the range of the underlying type of enumType.</exception>
        /// <exception cref="SqlNullValueException">Thrown exception if response value is <see cref="DBNull"/>.</exception>
        /// <typeparam name="TReceive">Type of data received.</typeparam>
        public static TReceive GetFieldValue<TReceive>(this DbDataReader reader, string name, TReceive defaultValue, bool throwDbNull = false, IFormatProvider provider = default)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            var ordinal = reader.GetOrdinal(name);
            return GetFieldValue(reader, ordinal, defaultValue, throwDbNull, provider);
        }
        /// <summary>
        /// Gets the value of the specified column as the requested type.
        /// </summary>
        /// <param name="reader">Database data reader.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="defaultValue">Return value if the column contains non-existent or missing values.</param>
        /// <param name="throwDbNull">Thrown exception if response value is <see cref="DBNull"/>.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <exception cref="ArgumentException">When converting to Enum: return value from the database is either an empty string or only contains white space. -or- Value is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="ArgumentNullException">Reader is null.</exception>
        /// <exception cref="IndexOutOfRangeException">The column index is out of range.</exception>
        /// <exception cref="InvalidOperationException">The connection drops or is closed during the data retrieval.The System.Data.Common.DbDataReader is closed during the data retrieval.There is no data ready to be read. Tried to read a previously-read column in sequential mode. There was an asynchronous operation in progress. This applies to all Get* methods when running in sequential mode, as they could be called while reading a stream.</exception>
        /// <exception cref="OverflowException">When converting to Enum: value is outside the range of the underlying type of enumType.</exception>
        /// <exception cref="SqlNullValueException">Thrown exception if response value is <see cref="DBNull"/>.</exception>
        /// <typeparam name="TReceive">Type of data received.</typeparam>
        public static TReceive GetFieldValue<TReceive>(this DbDataReader reader, int ordinal, TReceive defaultValue, bool throwDbNull = false, IFormatProvider provider = default)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            var value = reader.GetFieldValue<object>(ordinal);

            var isDbNull = reader.IsDBNull(ordinal);
            if (isDbNull && throwDbNull)
                throw new SqlNullValueException(string.Format(provider, Properties.Resources.SqlNullValueException, reader.GetName(ordinal)));

            return isDbNull
                ? defaultValue
                : typeof(TReceive).IsEnum
                    ? (TReceive)Enum.Parse(typeof(TReceive), value.ToString())
                    : typeof(IConvertible).IsAssignableFrom(typeof(TReceive))
                        ? (TReceive)Convert.ChangeType(value, typeof(TReceive), provider)
                        : default;
        }
        /// <summary>
        /// Asynchronously gets the value of the specified column as the requested type.
        /// </summary>
        /// <param name="reader">Database data reader.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="defaultValue">Return value if the column contains non-existent or missing values.</param>
        /// <param name="throwDbNull">Thrown exception if response value is <see cref="DBNull"/>.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <exception cref="ArgumentException">When converting to Enum: return value from the database is either an empty string or only contains white space. -or- Value is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="ArgumentNullException">Reader is null.</exception>
        /// <exception cref="IndexOutOfRangeException">The name specified is not a valid column name.</exception>
        /// <exception cref="InvalidOperationException">The connection drops or is closed during the data retrieval.The System.Data.Common.DbDataReader is closed during the data retrieval.There is no data ready to be read. Tried to read a previously-read column in sequential mode. There was an asynchronous operation in progress. This applies to all Get* methods when running in sequential mode, as they could be called while reading a stream.</exception>
        /// <exception cref="OverflowException">When converting to Enum: value is outside the range of the underlying type of enumType.</exception>
        /// <exception cref="SqlNullValueException">Thrown exception if response value is <see cref="DBNull"/>.</exception>
        /// <typeparam name="TReceive">Type of data received.</typeparam>
        public static Task<TReceive> GetFieldValueAsync<TReceive>(this DbDataReader reader, string name, TReceive defaultValue, bool throwDbNull = false, IFormatProvider provider = default, CancellationToken cancellationToken = default)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            var ordinal = reader.GetOrdinal(name);
            return GetFieldValueAsync(reader, ordinal, defaultValue, throwDbNull, provider, cancellationToken);
        }
        /// <summary>
        /// Asynchronously gets the value of the specified column as the requested type.
        /// </summary>
        /// <param name="reader">Database data reader.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="defaultValue">Return value if the column contains non-existent or missing values.</param>
        /// <param name="throwDbNull">Thrown exception if response value is <see cref="DBNull"/>.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <exception cref="ArgumentException">When converting to Enum: return value from the database is either an empty string or only contains white space. -or- Value is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="ArgumentNullException">Reader is null.</exception>
        /// <exception cref="IndexOutOfRangeException">The column index is out of range.</exception>
        /// <exception cref="InvalidOperationException">The connection drops or is closed during the data retrieval.The System.Data.Common.DbDataReader is closed during the data retrieval.There is no data ready to be read. Tried to read a previously-read column in sequential mode. There was an asynchronous operation in progress. This applies to all Get* methods when running in sequential mode, as they could be called while reading a stream.</exception>
        /// <exception cref="OverflowException">When converting to Enum: value is outside the range of the underlying type of enumType.</exception>
        /// <exception cref="SqlNullValueException">Thrown exception if response value is <see cref="DBNull"/>.</exception>
        /// <typeparam name="TReceive">Type of data received.</typeparam>
        public static async Task<TReceive> GetFieldValueAsync<TReceive>(this DbDataReader reader, int ordinal, TReceive defaultValue, bool throwDbNull = false, IFormatProvider provider = default, CancellationToken cancellationToken = default)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            var value = await reader.GetFieldValueAsync<object>(ordinal, cancellationToken).ConfigureAwait(false);

            var isDbNull = await reader.IsDBNullAsync(ordinal, cancellationToken).ConfigureAwait(false);
            if (isDbNull && throwDbNull)
                throw new SqlNullValueException(string.Format(provider, Properties.Resources.SqlNullValueException, reader.GetName(ordinal)));

            return isDbNull
                ? defaultValue
                : typeof(TReceive).IsEnum
                    ? (TReceive)Enum.Parse(typeof(TReceive), value.ToString())
                    : typeof(IConvertible).IsAssignableFrom(typeof(TReceive))
                        ? (TReceive)Convert.ChangeType(value, typeof(TReceive), provider)
                        : default;
        }
    }
}
