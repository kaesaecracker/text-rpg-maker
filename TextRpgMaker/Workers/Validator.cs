using System;
using System.Linq;
using System.Reflection;
using TextRpgMaker.Models;
using static Serilog.Log;
using Attribute = System.Attribute;

namespace TextRpgMaker.Workers
{
    public class Validator
    {
        private Project _project;

        public Validator(Project project)
        {
            this._project = project;
        }

        /// <summary>
        /// Runs all methods in Validator that have the [ValidationMethod] Attribute.
        /// 
        /// To write a new Validation, you can just add a new Method that throws a
        /// ValidationFailedException if the validation fails with the [ValidationMethod] Attribute.
        /// </summary>
        public void ValidateAll()
        {
            Logger.Information("VALIDATOR: Starting validation");
            var methods = (
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                from method in type.GetMethods()
                where method.GetParameters().Length == 1
                      && method.GetParameters()[0].ParameterType == typeof(Project)
                select method
            ).ToList();

            Logger.Debug("VALIDATOR: Number of validations: {nr}", methods.Count);
            foreach (var methodInfo in methods)
            {
                Logger.Debug("VALIDATOR: Running validation {class}.{method}",
                    methodInfo.DeclaringType.Name, methodInfo.Name);
                methodInfo.Invoke(this, new object[] {this._project});
            }
        }
    }

    public class ValidationFailedException : Exception
    {
        public ValidationFailedException(string msg, Exception inner = null) : base(msg, inner)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ValidatorClassAttribute : Attribute
    {
    }
}