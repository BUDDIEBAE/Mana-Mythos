using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "Npc/Npc Data")]
public class NpcNeedsData : ScriptableObject
{

    [SerializeField] private ItemDictionary _itemDictionary;
    [SerializeField] private ItemData[] _allItemPosible => _itemDictionary.Items;
    public NpcType _npcType;
    public List<ItemData> _itemToBuy = new List<ItemData>();
    public int rarity;



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
                rarity = 1;
                return new List<ItemType> { ItemType.Herb, ItemType.GemStone };

            case NpcType.TownsPeople:
                rarity = 1;
                return new List<ItemType> { ItemType.Artifact, ItemType.ArtifactParts, ItemType.Metal, ItemType.Herb, ItemType.Potion };

            case NpcType.Soldier:
                rarity = 2;
                return new List<ItemType> { ItemType.Potion, ItemType.Artifact };

            case NpcType.Noble:
                rarity = 3;
                return new List<ItemType> { ItemType.Accessory, ItemType.GemStone };

            case NpcType.Explorer:
                rarity = 2;
                return new List<ItemType> { ItemType.Potion, ItemType.Artifact };

            case NpcType.Mage:
                rarity = 2;
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
