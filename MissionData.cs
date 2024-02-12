using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melt", menuName = "Meltagochi/MissionData", order = 1)]
public class MissionData : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private string completeText;
    [SerializeField] private int minsDuration;
    [SerializeField] private int minsExpiration;
    [SerializeField] private List<InventoryObjectData> itemRewards;
    [SerializeField] private int expReward;
    [SerializeField] private int screamcoinReward;

    [SerializeField] private int sillyness;
    [SerializeField] private int kindness;
    [SerializeField] private int cuddliness;
    [SerializeField] private int friendliness;

    [SerializeField] private int appearanceLevel;

    [SerializeField] private List<VehicleData.VehicleTypes> thisTypes;

    [SerializeField] private List<float> appearanceTimes = new List<float> { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };

    public enum Rarity { Common, Uncommon, Rare, UltraRare }
    [SerializeField] private Rarity missionRarity;

    public Rarity GetRarity()
    {
        return missionRarity;
    }

    public List<float> GetAppearanceTimes()
    {
        return appearanceTimes;
    }

    public int GetLevel()
    {
        return appearanceLevel;
    }

    public Color GetColour()
    {
        return MissionAssigner.MissionToColour(GetRarity());
    }

    public string GetTitle()
    {
        return title;
    }
    public string GetDescription()
    {
        return description;
    }
    public string GetCompleteText()
    {
        return completeText;
    }

    public int GetDuration()
    {
        return minsDuration;
    }

    public int GetExpiration()
    {
        return minsExpiration;
    }
    public List<InventoryObjectData> GetItemRewards()
    {
        return itemRewards;
    }
    public int GetExpReward()
    {
        return expReward;
    }
    public int GetScreamcoinReward()
    {
        return screamcoinReward;
    }

    public List<VehicleData.VehicleTypes> GetTypes()
    {
        return thisTypes;
    }

    public int GetSillynessReq()
    {
        return sillyness;
    }
    public int GetCuddlinessReq()
    {
        return cuddliness;
    }
    public int GetKindnessReq()
    {
        return kindness;
    }
    public int GetFriendlinessReq()
    {
        return friendliness;
    }

}
