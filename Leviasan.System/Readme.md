### System.Common.Data
Optimization of storage and executing SQL statements.

#### How using with Oracle database:
Prepare:
```
    /// <summary>
    /// Represents an Oracle SQL statement that is executed while connected to a data source.
    /// </summary>
    /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
    /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
    public interface IOracleSqlStatement<TRequest, TResponse> : ISqlStatement<OracleParameterCollection, OracleDataReader, TRequest, TResponse>
    {
        /// <summary>
        /// Specifies the binding method in the collection. True if the parameters are bound by name. False if the parameters are bound by position.
        /// </summary>
        bool BindByName { get; }
    }
    /// <summary>
    /// Represents an Oracle SQL statement builder.
    /// </summary>
    /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
    /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
    public interface IOracleSqlStatementBuilder<TRequest, TResponse> : ISqlStatementBuilder<OracleParameterCollection, OracleDataReader, TRequest, TResponse>
    {
        /// <summary>
        /// Specifies the binding method in the collection. True if the parameters are bound by name. False if the parameters are bound by position.
        /// </summary>
        /// <param name="bindByName">Is the binding parameter by name.</param>
        IOracleSqlStatementBuilder<TRequest, TResponse> SetBindByName(bool bindByName);
    }
    /// <summary>
    /// Allows configuration for an Oracle SQL statement to be factored into a separate class.
    /// Implement this interface, applying configuration for the SQL statement in the <see cref="ISqlStatementDirector{TCollection, TReader, TBuilder, TRequest, TResponse}.Configure(TBuilder)"/> method,
    /// and then apply the configuration using <see cref="ISqlPackageBuilder.ApplyConfiguration{TCollection, TReader, TBuilder, TRequest, TResponse}(ISqlStatementDirector{TCollection, TReader, TBuilder, TRequest, TResponse})"/>.
    /// </summary>
    /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
    /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
    public interface IOracleSqlStatementDirector<TRequest, TResponse> : ISqlStatementDirector<OracleParameterCollection, OracleDataReader, OracleSqlStatementBuilder<TRequest, TResponse>, TRequest, TResponse>
    {
    }
    /// <summary>
    /// Represents an Oracle SQL package.
    /// </summary>
    public abstract class OracleSqlPackage : SqlPackage<OracleParameterCollection, OracleDataReader>
    {
        /// <summary>
        /// The database context.
        /// </summary>
        private readonly DbContext _context;

        /// <summary>
        /// Initializes a new instance of <see cref="OracleSqlPackage"/> class with specified database connection.
        /// </summary>
        /// <param name="context"></param>
        public OracleSqlPackage(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// The database connection.
        /// </summary>
        public override DbConnection Connection => _context?.Database.GetDbConnection();
    }
    /// <summary>
    /// Represents an Oracle SQL statement that is executed while connected to a data source.
    /// </summary>
    /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
    /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
    public sealed class OracleSqlStatement<TRequest, TResponse> : SqlStatement<OracleParameterCollection, OracleDataReader, TRequest, TResponse>, IOracleSqlStatement<TRequest, TResponse>
    {
        /// <summary>
        /// Specifies the binding method in the collection. True if the parameters are bound by name. False if the parameters are bound by position.
        /// </summary>
        public bool BindByName { get; set; }

        /// <summary>
        /// Creates full configured a new instance of <see cref="OracleCommand"/> class.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="request">The object that represents the request data.</param>
        public override DbCommand CreateDbCommand(DbConnection connection, TRequest request)
        {
            var command = (OracleCommand)base.CreateDbCommand(connection, request);
            command.BindByName = BindByName;
            return command;
        }
    }
    /// <summary>
    /// Represents an Oracle SQL statement builder.
    /// </summary>
    /// <typeparam name="TRequest">The data type describing the input parameters.</typeparam>
    /// <typeparam name="TResponse">The type of the result returned SQL statement.</typeparam>
    public sealed class OracleSqlStatementBuilder<TRequest, TResponse> : SqlStatementBuilder<OracleParameterCollection, OracleDataReader, TRequest, TResponse>, IOracleSqlStatementBuilder<TRequest, TResponse>
    {
        /// <summary>
        /// Object to construction.
        /// </summary>
        private readonly OracleSqlStatement<TRequest, TResponse> _statement;

        /// <summary>
        /// Initializes a new instance of <see cref="OracleSqlStatementBuilder{TRequest, TResponse}"/> class.
        /// </summary>
        public OracleSqlStatementBuilder()
        {
            _statement = new OracleSqlStatement<TRequest, TResponse>();
        }

        /// <summary>
        /// Returns the SQL statement.
        /// </summary>
        public override ISqlStatement<OracleParameterCollection, OracleDataReader, TRequest, TResponse> Build()
        {
            var internalStatement = base.Build();
            _statement.CommandText = internalStatement.CommandText;
            _statement.CommandType = internalStatement.CommandType;
            _statement.Parameters = internalStatement.Parameters;
            _statement.Reader = internalStatement.Reader;
            _statement.ReaderAsync = internalStatement.ReaderAsync;

            return _statement;
        }
        /// <summary>
        /// Specifies the binding method in the collection. True if the parameters are bound by name. False if the parameters are bound by position.
        /// </summary>
        /// <param name="bindByName">Is the binding parameter by name.</param>
        public IOracleSqlStatementBuilder<TRequest, TResponse> SetBindByName(bool bindByName)
        {
            _statement.BindByName = bindByName;
            return this;
        }
    }
```
Implementation:
```
    /// <summary>
    /// Represents a text command that gets session-id with max expire date by username.
    /// </summary>
    public sealed class ExampleSqlStatement : IOracleSqlStatementDirector<int, string>
    {
        /// <summary>
        /// The unique key.
        /// </summary>
        public string Key => nameof(ExampleSqlStatement);

        /// <summary>
        /// Configures a SQL statement.
        /// </summary>
        /// <param name="builder">Oracle SQL statement builder.</param>
        public void Configure(OracleSqlStatementBuilder<int, string> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder
                .SetBindByName(true)
                .SetCommandText("select name from table where t.id = :id")
                .SetCommandType(CommandType.Text)
                .AddParameters((parameters, request) =>
                {
                    parameters.Add("id", OracleDbType.Decimal).Value = 1;
                })
                .AddReader(reader =>
                {
                    string name = null;
                    if (reader.Read())
                        name = reader.GetString("name");
                    return name;
                });
        }
    }
    /// <summary>
    /// Example Sql Package.
    /// </summary>
    public sealed class ExampleSqlPackage : OracleSqlPackage
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ExampleSqlPackage"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public MSUnitsSqlPackage(ApplicationDbContext context) : base(context?.Database.GetDbConnection()) { }

        /// <summary>
        /// Get name from table.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public string GetName(int id)
        {
            var response = ExecuteReader<int, string>(
                key: nameof(ExampleSqlStatement),
                request: id);

            return response;
        }

        /// <summary>
        /// The registration of <see cref="IOracleSqlStatementDirector{TRequest, TResponse}"/> object.
        /// </summary>
        /// <param name="builder">The SQL package builder.</param>
        protected override void OnInitializeSqlStatements(ISqlPackageBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.ApplyConfiguration(new ExampleSqlStatement());
        }
    }
```
If using the .NET Core web project, you need to register in the "startup" class "ExampleSqlPackage". And get it in the controller.
```
    public void ConfigureServices(IServiceCollection services)
    {
        ...
        services.AddScoped<ExampleSqlPackage>();
    }
```