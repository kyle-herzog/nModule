using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace nModule.Utilities
{
    /// <summary>
    /// Provides common utility methods for Reflection
    /// </summary>
    public static class ReflectionUtility
    {
        #region Classes

        private class ReflectionDefinition
        {
            Type Type { get; set; }
            public Dictionary<Type, MemberInfo[]> AttributedMembers { get; set; }

            public ReflectionDefinition(Type type)
            {
                Type = type;
                AttributedMembers = new Dictionary<Type, MemberInfo[]>();
            }
        }

        #endregion

        /// <summary>
        /// Assembly file extnesion
        /// </summary>
        public const string AssemplyExtension = "*.dll";

        private static Dictionary<Type, ReflectionDefinition> _reflectionDefinitions = new Dictionary<Type, ReflectionDefinition>();

        /// <summary>
        /// Returns all assemblies found with the given criteria.
        /// </summary>
        /// <param name="includeFileSystem">Whether or not the search should go to finding files within the file system where the application resides or just retrieve assemblies already loaded within the AppDomain.</param>
        /// <param name="searchOption">Whether or not the search for assemblies should include all directories or just the top most directory.</param>
        /// <param name="includedPaths">Directories that should be included within the search for assemblies.</param>
        /// <returns>A collection of Assemblies mapped to thier paths</returns>
        public static IDictionary<string, Assembly> GetAssemblies(bool includeFileSystem = false, SearchOption searchOption = SearchOption.TopDirectoryOnly, params string[] includedPaths)
        {
            var assemblies = new Dictionary<string, Assembly>();
            var searchPaths = new List<string>();
            if(includeFileSystem)
                searchPaths.Add(ApplicationInfo.Directory);
            if(includedPaths != null)
                searchPaths.AddRange(includedPaths);
            searchPaths.ForEach(path =>
            {
                var files = Directory.GetFiles(path, AssemplyExtension, searchOption);
                files.ToList().ForEach(file => assemblies.Add(file, Assembly.LoadFrom(file)));
            });
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                assemblies[assembly.Location] = assembly;
            }
            return assemblies;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assignableType"></param>
        /// <returns></returns>
        public static Type[] LoadAssignableType(Type assignableType)
        {
            var types = new List<Type>();
            foreach (var kvp in GetAssemblies())
            {
                try
                {
                    var assembly = kvp.Value;
                    var assemblyTypes = assembly.GetTypes();
                    types.AddRange(assemblyTypes.Where(type => !type.IsAbstract && assignableType.IsAssignableFrom(type)));
                }
                catch (Exception ex)
                {
                    var myException = new Exception("Error loading assignable types", ex);
                    myException.Data.Add("Assembly Name", kvp.Value.FullName);
                    myException.Data.Add("Assignable Type", assignableType.FullName);
                    throw myException;
                }
            }
            return types.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static MemberInfo[] LoadAttributedMembers(object entity, Type attributeType)
        {
            return LoadAttributedMembers(entity.GetType(), attributeType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static MemberInfo[] LoadAttributedMembers(Type entityType, Type attributeType)
        {
            return LoadAttributedMembers(entityType, attributeType, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="attributeType"></param>
        /// <param name="inheritedAttributes"></param>
        /// <returns></returns>
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
                var members = new List<MemberInfo>();
                foreach (var member in entityType.GetMembers())
                {
#if DEBUG
                    Console.WriteLine("\tMember: {0}", member.Name);
#endif
                    var attributes = member.GetCustomAttributes(attributeType, inheritedAttributes);
                    if (attributes != null && attributes.Length > 0)
                        members.Add(member);
#if DEBUG
                    foreach (var attribute in attributes)
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
