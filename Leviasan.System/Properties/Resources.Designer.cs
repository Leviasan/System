﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace System.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("System.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One of the next required methods is not called: {0}.
        /// </summary>
        internal static string BuilderException {
            get {
                return ResourceManager.GetString("BuilderException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Key: {0} is not found..
        /// </summary>
        internal static string KeyNotFoundException {
            get {
                return ResourceManager.GetString("KeyNotFoundException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value {0} is null..
        /// </summary>
        internal static string SqlNullValueException {
            get {
                return ResourceManager.GetString("SqlNullValueException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The SQL statement is not fully configured. Invalid property state: {0}..
        /// </summary>
        internal static string SqlStatementInvalidState {
            get {
                return ResourceManager.GetString("SqlStatementInvalidState", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified string can not be null, empty, or consists only of white-space characters..
        /// </summary>
        internal static string StringIsMissing {
            get {
                return ResourceManager.GetString("StringIsMissing", resourceCulture);
            }
        }
    }
}
