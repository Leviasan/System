namespace System.DirectoryServices.AccountManagement
{
    /// <summary>
    /// Represents of principal context options.
    /// </summary>
    public sealed class PrincipalContextOptions
    {
        /// <summary>
        /// Specifies the type of store to which the principal belongs.
        /// </summary>
        public ContextType ContextType { get; set; }
        /// <summary>
        /// The name of the domain or server for <see cref="ContextType.Domain"/>
        /// context types, the machine name for <see cref="ContextType.Machine"/> 
        /// context types, or the name of the server and port hosting the <see cref="ContextType.ApplicationDirectory"/>
        /// instance. If the name is null for a <see cref="ContextType.Domain"/> 
        /// context type this context is a domain controller for the domain of the user principal
        /// under which the thread is running. If the name is null for a <see cref="ContextType.Machine"/>
        /// context type, this is the local machine name. This parameter cannot be null for
        /// <see cref="ContextType.ApplicationDirectory"/> context types.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The container on the store to use as the root of the context. All queries are
        /// performed under this root, and all inserts are performed into this container.
        /// For <see cref="ContextType.Domain"/> and <see cref="ContextType.ApplicationDirectory"/>
        /// context types, this parameter is the distinguished name of a container object.
        /// For <see cref="ContextType.Machine"/> context types, this parameter must be set to null.
        /// </summary>
        public string Container { get; set; }
        /// <summary>
        /// A combination of one or more <see cref="System.DirectoryServices.AccountManagement.ContextOptions"/>
        /// enumeration values the options used to bind to the server. If this parameter
        /// is null, the default options are <see cref="ContextOptions.Negotiate"/> | <see cref="ContextOptions.Signing"/> | <see cref="ContextOptions.Sealing"/>.
        /// </summary>
        public ContextOptions ContextOptions { get; set; }
        /// <summary>
        /// The username used to connect to the store. If the username and password parameters
        /// are both null, the default credentials of the current principal are used. Otherwise,
        /// both username and password must be non-null, and the credentials they specify
        /// are used to connect to the store.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The password used to connect to the store. If the username and password parameters
        /// are both null, the default credentials of the current principal are used. Otherwise,
        /// both username and password must be non-null, and the credentials they specify
        /// are used to connect to the store.
        /// </summary>
        public string Password { get; set; }
    }
}
