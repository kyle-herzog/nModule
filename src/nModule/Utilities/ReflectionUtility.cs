using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace nModule.Utilities
{
    public static class ReflectionUtility
    {
        #region Classes

        private class ReflectionDefinition
        {
            public Type Type { get; set; }
            public Dictionary<Type, MemberInfo[]> AttributedMembers { get; set; }

            public ReflectionDefinition(Type type)
            {
                Type = type;
                AttributedMembers = new Dictionary<Type, MemberInfo[]>();
            }
        }

        #endregion

        private static Dictionary<Type, ReflectionDefinition> _reflectionDefinitions = new Dictionary<Type, ReflectionDefinition>();

        public static IDictionary<string, Assembly> GetAssemblies()
        {
            return GetAssemblies(true, SearchOption.AllDirectories);
        }

        public static IDictionary<string, Assembly> GetAssemblies(bool includeFileSystem, SearchOption searchOption)
        {
            Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();
            if (includeFileSystem)
            {
                foreach (string dll in Directory.GetFiles(ApplicationInfo.Directory, "*.dll", searchOption))
                {
                    try
                    {
                        assemblies.Add(dll, Assembly.LoadFrom(dll));
                    }
                    catch { }
                }
            }
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                assemblies[assembly.Location] = assembly;
            }
            return assemblies;
        }

        public static Type[] LoadAssignableType(Type assignableType)
        {
            List<Type> types = new List<Type>();
            foreach (KeyValuePair<string, Assembly> kvp in GetAssemblies())
            {
                try
                {
                    Assembly assembly = kvp.Value;
                    Type[] assemblyTypes = assembly.GetTypes();
                    foreach (Type type in assemblyTypes)
                    {
                        if (!type.IsAbstract && assignableType.IsAssignableFrom(type))
                        {
                            types.Add(type);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception myException = new Exception("Error loading assignable types", ex);
                    myException.Data.Add("Assembly Name", kvp.Value.FullName);
                    myException.Data.Add("Assignable Type", assignableType.FullName);
                    throw myException;
                }
            }
            return types.ToArray();
        }

        public static MemberInfo[] LoadAttributedMembers(object entity, Type attributeType)
        {
            return LoadAttributedMembers(entity.GetType(), attributeType);
        }

        public static MemberInfo[] LoadAttributedMembers(Type entityType, Type attributeType)
        {
            return LoadAttributedMembers(entityType, attributeType, false);
        }

        public static MemberInfo[] LoadAttributedMembers(Type entityType, Type attributeType, bool inheritedAttributes)
        {
            ReflectionDefinition definition;
            if (!_reflectionDefinitions.TryGetValue(entityType, out definition))
            {
                definition = new ReflectionDefinition(entityType);
                _reflectionDefinitions[entityType] = definition;
            }
#if DEBUG
            Console.WriteLine("Loading Members for {0}", entityType.FullName);
#endif
            if (definition.AttributedMembers.ContainsKey(attributeType))
            {
                return definition.AttributedMembers[attributeType];
            }
            else
            {
                List<MemberInfo> members = new List<MemberInfo>();
                foreach (MemberInfo member in entityType.GetMembers())
                {
#if DEBUG
                    Console.WriteLine("\tMember: {0}", member.Name);
#endif
                    object[] attributes = member.GetCustomAttributes(attributeType, inheritedAttributes);
                    if (attributes != null && attributes.Length > 0)
                        members.Add(member);
#if DEBUG
                    foreach (object attribute in attributes)
                    {
                        Console.WriteLine("\t\tAttribute: {0}", attribute.GetType().FullName);
                    }
#endif
                }

                return members.ToArray();
            }
        }
    }
}
