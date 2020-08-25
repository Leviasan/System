### System.Common.Data
Optimization of storage and executing SQL statements.

#### How using with Oracle database:
Prepare:
```csharp
   
    public interface IOracleSqlStatement<TRequest, TResponse> : ISqlStatement<OracleParameterCollection, OracleDataReader, TRequest, TResponse>
    {
        bool BindByName { get; }
    }
    public interface IOracleSqlStatementBuilder<TRequest, TResponse> : ISqlStatementBuilder<OracleParameterCollection, OracleDataReader, TRequest, TResponse>
    {
        IOracleSqlStatementBuilder<TRequest, TResponse> SetBindByName(bool bindByName);
    }
    public interface IOracleSqlStatementDirector<TRequest, TResponse> : ISqlStatementDirector<OracleParameterCollection, OracleDataReader, OracleSqlStatementBuilder<TRequest, TResponse>, TRequest, TResponse>
    {
    }
    public abstract class OracleSqlPackage : SqlPackage<OracleParameterCollection, OracleDataReader>
    {
        private readonly DbContext _context;

        public OracleSqlPackage(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public override DbConnection Connection => _context?.Database.GetDbConnection();
    }
    public sealed class OracleSqlStatement<TRequest, TResponse> : SqlStatement<OracleParameterCollection, OracleDataReader, TRequest, TResponse>, IOracleSqlStatement<TRequest, TResponse>
    {
        public bool BindByName { get; set; }

        public override DbCommand CreateDbCommand(DbConnection connection, TRequest request)
        {
            var command = (OracleCommand)base.CreateDbCommand(connection, request);
            command.BindByName = BindByName;
            return command;
        }
    }
    public sealed class OracleSqlStatementBuilder<TRequest, TResponse> : SqlStatementBuilder<OracleParameterCollection, OracleDataReader, TRequest, TResponse>, IOracleSqlStatementBuilder<TRequest, TResponse>
    {
        private readonly OracleSqlStatement<TRequest, TResponse> _statement;

        public OracleSqlStatementBuilder()
        {
            _statement = new OracleSqlStatement<TRequest, TResponse>();
        }

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
        public IOracleSqlStatementBuilder<TRequest, TResponse> SetBindByName(bool bindByName)
        {
            _statement.BindByName = bindByName;
            return this;
        }
    }
```
Implementation:
```csharp
    public sealed class GetUsernameSqlStatement : IOracleSqlStatementDirector<int, string>
    {
        public string Key => nameof(GetUsernameSqlStatement);

        public void Configure(OracleSqlStatementBuilder<int, string> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder
                .SetBindByName(true)
                .SetCommandText("select name from table users where t.id = :id")
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
    public sealed class UsersSqlPackage : OracleSqlPackage
    {
        public UsersSqlPackage(ApplicationDbContext context) : base(context) { }

        public string GetUsername(int id)
        {
            var response = ExecuteReader<int, string>(
                key: nameof(GetUsernameSqlStatement),
                request: id);

            return response;
        }

        protected override void OnInitializeSqlStatements(ISqlPackageBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.ApplyConfiguration(new GetUsernameSqlStatement());
        }
    }
    public sealed class ApplicationDbContext : DbContext 
    {
         public ApplicationDbContext(DbContextOptions options) : base(options) 
         {
            UsersSqlPackage = new UsersSqlPackage(this);
         }

         public UsersSqlPackage UsersSqlPackage { get; }
    }
```
If using the .NET Core web project, you need to register in the "startup" class "ApplicationDbContext". And get it in the controller.
```csharp
    [ApiController]
    [Route("api/[controller]")]
    public sealed class ExampleController : ControllerBase 
    {
        private readonly ApplicationDbContext _context;

        public ExampleController(ApplicationDbContext context)
        {
           _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet("{id:int}")]
        public IActionResult GetUsername(int id) 
        {
            var username = _context.UsersSqlPackage.GetUsername(id);
            return Ok(username);
        }
    }
```
