using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using TextRpgMaker.Models;
using static Serilog.Log;

namespace TextRpgMaker.Workers
{
    public class Validator
    {
        private Project _project;

        public Validator(Project project)
        {
            this._project = project;
        }

        public void ValidateAll()
        {
            Logger.Information("VALIDATOR: Starting validation");
            var methods = (
                from m in this.GetType().GetMethods(
                    BindingFlags.NonPublic | BindingFlags.Instance)
                let att = m.GetCustomAttribute<ValidatorMethodAttribute>()
                where att != null && m.GetParameters().Length == 0
                select m
            ).ToList();

            Logger.Debug("VALIDATOR: Number of validations: {nr}", methods.Count);
            foreach (var methodInfo in methods)
            {
                Logger.Debug("VALIDATOR: Running validation {methodName}", methodInfo.Name);
                methodInfo.Invoke(this, new object[] { });
            }
        }

        [ValidatorMethod]
        private void StartInfoIdsExist()
        {
            var missingChars = (
                from charId in this._project.Info.StartInfo.CharacterIds
                let characters = this._project.Characters.Select(c => c.Id == charId)
                where characters.Count() != 0
                select charId
            ).ToList();

            if (missingChars.Count == 0) return;
            throw new ValidationFailedException("Start characters do not exist: " +
                                                missingChars.Aggregate((curr, id) => $"{curr}, {id}"));
            
            // todo start scene
            // todo start dialog
        }

        [ValidatorMethod]
        private void CharacterDropsExist()
        {
            // todo character drops exist
        }

        [ValidatorMethod]
        private void CharacterDialogsExist()
        {
            // todo characterdialogsexist
        }        
        
        public class ValidationFailedException : Exception
        {
            public ValidationFailedException(string msg, Exception inner = null) : base(msg, inner)
            {
            }
        }
    }
}