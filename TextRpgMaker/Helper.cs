using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TextRpgMaker
{
    public static class Helper
    {
        // todo remove / replace
        public static List<(Type type, string pathInProj, bool required, bool isList)>
            TypesToLoad() => (
            from assembly in AppDomain.CurrentDomain.GetAssemblies()
            from type in assembly.GetTypes()
            where type.IsDefined(typeof(LoadFromProjectFileAttribute), inherit: false)
            let fileAnnotation = type.GetCustomAttribute<LoadFromProjectFileAttribute>()
            select (
                Type: type,
                fileAnnotation.ProjectRelativePath,
                fileAnnotation.Required,
                fileAnnotation.IsList
            )
        ).ToList();

        // todo to helper method with parameter
        public static string ProjectToNormalPath(string pathInProj, string pathToProj) =>
            pathToProj + "/" + pathInProj;
    }
}