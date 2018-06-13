using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;

namespace TextRpgMaker
{
    public static class Helper
    {
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

        public static string ProjectToNormalPath(this string pathInProj, string pathToProj = null)
        {
            return (pathToProj ?? AppState.LoadedProject.ProjectDir)
                   + "/" + pathInProj;
        }
    }
}