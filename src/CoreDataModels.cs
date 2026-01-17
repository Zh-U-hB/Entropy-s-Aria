using System;
using System.Collections.Generic;
using UnityEngine;

// 属性类型：生存 / 科技 / 信仰
public enum StatType
{
    Survival,
    Tech,
    Faith
}

// 卡牌类型：玩家卡 / 文明行动卡
public enum CardType
{
    GodPower,
    CivAction
}

// 单个属性修改效果
[Serializable]
public struct StatModifier
{
    public StatType targetStat;
    public int value;
}

// 卡牌静态数据
[Serializable]
[CreateAssetMenu(fileName = "NewCard", menuName = "CivilizationPetriDish/Card")]
public class CardData : ScriptableObject
{
    public string id;
    public string cardName;
    [TextArea]
    public string description;
    public CardType type;
    public List<StatModifier> effects = new List<StatModifier>();
}

// 文明运行时数据
[Serializable]
public class Civilization
{
    public string id;
    public string civilizationName;
    
    [TextArea]
    public string personalityPrompt;

    // --- 修改点：将 Dictionary 改为直接的字段，方便 Unity 调试和 AI 读取 ---
    [Range(0, 100)] public int survival = 50;
    [Range(0, 100)] public int tech = 50;
    [Range(0, 100)] public int faith = 50;

    // 当前文明可用的手牌
    public List<CardData> handCards;
    public bool isAlive;

    public Civilization(string id, string civilizationName, string personalityPrompt)
    {
        this.id = id;
        this.civilizationName = civilizationName;
        this.personalityPrompt = personalityPrompt;
        isAlive = true;
        handCards = new List<CardData>();
        
        // 初始数值
        survival = 50;
        tech = 10;
        faith = 50;
    }

    // 修改指定属性并限制在 0–100 区间
    public void ModifyStat(StatType type, int amount)
    {
        switch (type)
        {
            case StatType.Survival:
                survival = Mathf.Clamp(survival + amount, 0, 100);
                break;
            case StatType.Tech:
                tech = Mathf.Clamp(tech + amount, 0, 100);
                break;
            case StatType.Faith:
                faith = Mathf.Clamp(faith + amount, 0, 100);
                break;
        }

        // 简单的死亡判定
        if (survival <= 0)
        {
            isAlive = false;
            survival = 0;
            Debug.Log($"{civilizationName} 已灭亡！");
        }
    }
}