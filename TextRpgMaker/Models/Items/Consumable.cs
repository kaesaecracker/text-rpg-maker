namespace TextRpgMaker.Models.Items
{
    [LoadFromProjectFile("items/consumables.yaml", required: false, isList: true)]
    public class Consumable : Element
    {
    }
}