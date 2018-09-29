using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.ProjectModels
{
    [DocumentedType]
    public class ChangeScene
    {
        [YamlMember(Alias="id")]
        [YamlProperties(true)]
        public string TargetScene { get; set; }
        
        [YamlMember(Alias="add-character")]
        public string PersonToAdd { get; set; }
        
        [YamlMember(Alias = "remove-person")]
        public string PersonToRemove { get; set; }
        
        [YamlMember(Alias = "add-connection-to")]
        public string SceneToConnect { get; set; }
        
        [YamlMember(Alias = "remove-connection-to")]
        public string SceneToDisconnect { get; set; }

        public void Apply()
        {
            // TODO Validations that check whether those scenes / characters actually exist
            var scene = AppState.Project.Scenes.GetId(this.TargetScene);
            
            if (!string.IsNullOrWhiteSpace(this.PersonToAdd))
                scene.Characters.Add(this.PersonToAdd);
            
            if (!string.IsNullOrWhiteSpace(this.PersonToRemove))
                scene.Characters.Remove(this.PersonToRemove);
            
            if (!string.IsNullOrWhiteSpace(this.SceneToConnect))
                scene.Connections.Add(this.SceneToConnect);

            if (!string.IsNullOrWhiteSpace(this.SceneToDisconnect))
                scene.Connections.Remove(this.SceneToDisconnect);
        }
    }
}