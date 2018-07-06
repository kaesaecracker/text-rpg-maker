using System;
using System.Linq;
using System.Reflection;
using TextRpgMaker.FileModels;
using TextRpgMaker.ProjectModels;
using static Serilog.Log;

namespace TextRpgMaker.Workers
{
    public static class ProjectValidator
    {
        /// <summary>
        ///     Runs all methods in Validator that have the [ValidationMethod] Attribute.
        ///     To write a new Validation, you can just add a new Method that throws a
        ///     ValidationFailedException if the validation fails with the [ValidationMethod] Attribute.
        /// </summary>
        public static void RunAllValidations(ProjectModel p)
        {
            Logger.Information("VALIDATOR: Starting validation");
            var methods = (
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                let attribute = type.GetCustomAttribute<ValidatorClassAttribute>()
                where attribute != null
                from method in type.GetMethods()
                where method.GetParameters().Length == 1
                      && method.GetParameters()[0].ParameterType == typeof(ProjectModel)
                select method
            ).ToList();

            Logger.Debug("VALIDATOR: Number of validations: {nr}", methods.Count);
            foreach (var methodInfo in methods)
            {
                Logger.Debug("VALIDATOR: Running validation {class}.{method}",
                    methodInfo.DeclaringType.Name, methodInfo.Name);
                methodInfo.Invoke(null, new object[] {p});
            }
        }
    }

    public class ValidationFailedException : Exception
    {
        public ValidationFailedException(string msg, Exception inner = null) : base(msg, inner)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ValidatorClassAttribute : Attribute
    {
    }
}