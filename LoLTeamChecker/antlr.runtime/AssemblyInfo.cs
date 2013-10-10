using System;
using System.Reflection;
using System.Runtime.CompilerServices;


// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

// TODO: Review the values of the assembly attributes

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Revision
//      Build Number
//
// You can specify all the values or you can default the Revision and Build Numbers
// by using the '*' as shown below:

#if STRONGNAME
#pragma warning disable 1699
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("org.antlr.snk")]
#pragma warning restore 1699
#endif

#if APTC
#endif