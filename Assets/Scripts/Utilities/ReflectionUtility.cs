using System;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionUtility
{
    public static List<string> namespaceWhitelist = new List<string>() { "ElysiumGraphs", "ElysiumUtilities" };
    public static List<string> namespaceBlacklist = new List<string>() { "UnityEditor", "UnityEngine" };

    public static List<Type> GetAllTypesWithWhitelist(List<string> _namespaceWhitelist)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        List<Type> whiteListedTypes = new List<Type>();

        //Get All types in specific namespace
        foreach (var assembly in assemblies)
        {
            whiteListedTypes.AddRange
            (assembly.GetTypes().Where
                (
                    _type => _namespaceWhitelist.Exists(_x => _x == _type.Namespace)
                             && _type.Namespace != "Null"
                    //||  _type.IsSubclassOf(typeof(UnityEngine.Object)))
                    //&&	!namespaceBlacklist.Contains(_type.Namespace.Split('.')[0])
                )
            );
        }

        //Remove weird "type+<>c__DisplayClass" thing
        whiteListedTypes.RemoveAll(_x => _x.DeclaringType != null);

        return whiteListedTypes;
    }
}
