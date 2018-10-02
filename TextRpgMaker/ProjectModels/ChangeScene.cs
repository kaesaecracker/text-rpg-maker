using System;
using System.Collections.Generic;
using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.ProjectModels
{
    /// <summary>
    /// Represents a change-scene element in a dialog choice.
    /// </summary>
    [DocumentedType]
    public class ChangeScene
    {
        [YamlMember(Alias = "id")]
        [YamlProperties(true)]
        public string TargetScene { get; set; }

        [YamlMember(Alias = "add-character")]
        public string PersonToAdd { get; set; }

        [YamlMember(Alias = "remove-character")]
        public string PersonToRemove { get; set; }

        [YamlMember(Alias = "add-connection-to")]
        public string SceneToConnect { get; set; }

        [YamlMember(Alias = "remove-connection-to")]
        public string SceneToDisconnect { get; set; }

        [YamlMember(Alias = "remove-item")]
        public ItemGrouping ItemToRemove { get; set; }

        [YamlMember(Alias = "add-item")]
        public ItemGrouping ItemToAdd { get; set; }

        /// <summary>
        /// Change scene according to properties
        /// </summary>
        public void Apply()
        {
            if (this.TargetScene == null)
                throw new ArgumentException("TargetScene property has to be set");

            // TODO Validations that check whether those scenes / characters actually exist
            var scene = AppState.Project.Scenes.GetId(this.TargetScene);

            if (!string.IsNullOrWhiteSpace(this.PersonToAdd))
                scene.CharacterIds.Add(this.PersonToAdd);

            if (!string.IsNullOrWhiteSpace(this.PersonToRemove))
                scene.CharacterIds.Remove(this.PersonToRemove);

            if (!string.IsNullOrWhiteSpace(this.SceneToConnect))
                scene.ConnectionIds.Add(this.SceneToConnect);

            if (!string.IsNullOrWhiteSpace(this.SceneToDisconnect))
                scene.ConnectionIds.Remove(this.SceneToDisconnect);

            // remove item if necessary
            if (this.ItemToRemove != null && scene.Items != null)
            {
                foreach (var ig in scene.Items)
                {
                    // search match
                    if (ig.ItemId != this.ItemToRemove.ItemId) continue;

                    // no items left -> remove from list
                    if (ig.Count - this.ItemToRemove.Count <= 0)
                    {
                        scene.Items.Remove(ig);
                        break;
                    }

                    // items left -> subtract
                    ig.Count -= this.ItemToRemove.Count;
                    break;
                }
            }

            // add item if necessary
            if (this.ItemToAdd != null)
            {
                if (scene.Items == null) scene.Items = new List<ItemGrouping>();

                // check if already exists
                bool found = false;
                foreach (var ig in scene.Items)
                {
                    // search match
                    if (ig.ItemId != this.ItemToAdd.ItemId) continue;

                    // found match -> increase count
                    ig.Count += this.ItemToAdd.Count;
                    found = true;
                    break;
                }

                // add if not found
                if (!found) scene.Items.Add(this.ItemToAdd);
            }
        }
    }
}