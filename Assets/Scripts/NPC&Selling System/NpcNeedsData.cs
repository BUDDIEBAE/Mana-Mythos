using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "Npc/Npc Data")]
public class NpcNeedsData : ScriptableObject
{
    [SerializeField] private ItemData[] _allItemPosible;
    public NpcType _npcType;
    public List<ItemData> _itemToBuy = new List<ItemData>();

    [ContextMenu("Generate Items To Buy")]

    private void OnValidate()
    {
        GenerateItemsToBuy();
    }

    public void GenerateItemsToBuy()
    {
        var desiredTypes = GetItemTypesForNpc(_npcType);

        _itemToBuy = _allItemPosible
                    .Where(item => desiredTypes.Contains(item.itemType))
                    .ToList();

        if(_npcType == NpcType.Noble)
        {
            _itemToBuy.AddRange(_allItemPosible
                                .Where(item => item.itemName == "Gold" && item.ID == "005"));
        }
    }

    private List<ItemType> GetItemTypesForNpc(NpcType type)
    {
        switch (type)
        {
            case NpcType.MageStudent:
                return new List<ItemType> { ItemType.Herb, ItemType.GemStone };

            case NpcType.TownsPeople:
                return new List<ItemType> { ItemType.Artifact, ItemType.ArtifactParts, ItemType.Metal, ItemType.Herb, ItemType.Potion };

            case NpcType.Soldier:
                return new List<ItemType> { ItemType.Potion, ItemType.Artifact };

            case NpcType.Noble:
                return new List<ItemType> { ItemType.Accessory, ItemType.GemStone };

            case NpcType.Explorer:
                return new List<ItemType> { ItemType.Potion, ItemType.Artifact };

            case NpcType.Mage:
                return new List<ItemType> { ItemType.Herb, ItemType.GemStone, ItemType.Metal };

            default:
                return new List<ItemType>();
        }
    }

}

public enum NpcType
{
    MageStudent,
    TownsPeople,
    Soldier,
    Noble,
    Explorer,
    Mage
}