using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Util;

public class Define
{
    public static readonly Dictionary<Type, Array> _enumDict = new Dictionary<Type, Array>();


    public static readonly float[] SUPPORTSKILL_GRADE_PROB = new float[]
    {
        0.4f,   // Common 확률
        0.4f,   // Uncommon 확률
        0.1f,   // Advanced 확률
        0.07f,  // Rare 확률
        0.03f,  // Legend 확률
    };

    #region 보석 경험치 획득량
    public const int SMALL_EXP_AMOUNT = 1;
    public const int GREEN_EXP_AMOUNT = 2;
    public const int BLUE_EXP_AMOUNT = 5;
    public const int YELLOW_EXP_AMOUNT = 10;
    #endregion

    #region 디폴트 장비/케릭터 아이디
    public const int CHARACTER_DEFAULT_ID = 201000;
    public const string WEAPON_DEFAULT_ID = "NW0101";
    public const string GLOVES_DEFAULT_ID = "NG0101";
    public const string RING_DEFAULT_ID = "NR0201";
    public const string HELMET_DEFAULT_ID = "NH0101";
    public const string ARMOR_DEFAULT_ID = "NA0101";
    public const string BOOTS_DEFAULT_ID = "NB0101";
    #endregion

    public static readonly int UI_GAMESCENE_SORT_ORDER = 301;
    #region Enum
    
    public enum ObjectType
    {
        MaterialToken,
        NatureToken,
        FactorialToken,
        EnemyToken,
        Null,
    }
    
    public enum TokenType
    {
        MaterialToken,
        NatureToken,
        FactorialToken,
        EnemyToken,
        Null,
    }
    
    public enum MaterialType
    {
        Gold,
        Dia,
        Stamina,
        Weapon,
        Gloves,
        Ring,
        Helmet,
        Armor,
        Boots,
        BronzeKey,
        SilverKey,
        GoldKey,
    }
    public enum SupportSkillName
    {
        Critical,
        MaxHpBonus,
        ExpBonus,
        SoulBonus,
        DamageReduction,
        AtkBonusRate,
        MoveBonusRate,
        EnergeBolt,
        IceBolt,
        PoisonField,
        EletronicField,
        Meteo,
        FrozenHeart,
        WindCutter
    }
    public enum SupportSkillGrade
    {
        Common,
        Uncommon,
        Advanced,
        Rare,
        Legend
    }

    public enum SupportSkillType
    {
        General,
        Special
    }
    //장비아이템에서 인벤토리에 있는지 케릭터 장비 에 있는지
    public enum UI_ItemParentType
    {
        CharacterEquipmentGroup,
        EquipInventoryGroup,
    }

    public enum GachaRarity
    {
        Normal,
        Special,
    }

    public enum EquipmentType
    {
        Weapon,
        Gloves,
        Ring,
        Helmet,
        Armor,
        Boots,
    }

    public enum EquipmentGrade
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Epic1,
        Epic2,
        Legendary,
        Legendary1,
        Legendary2,
        Legendary3,
        Myth,
        Myth1,
        Myth2,
        Myth3
    }

    public enum EquipmentSortType
    {
        Level,
        Grade,
    }

    public enum CreatureState
    {
        Idle,
        Skill,
        Moving,
        OnDamaged,
        Dead
    }


    public enum PlayerState
    {
        Idle,
        Run,
        Attack,
        Jump,
        Die
    }

    public enum Skill
    {
        EnergyBolt = 10001,       //100001 ~ 100005 
        IceBolt = 10011,          //100011 ~ 100015 
        PoisonField = 10021,      //100021 ~ 100025 
        EletronicField = 10031,   //100031 ~ 100035 
        Meteor = 10041,           //100041 ~ 100045 
        FrozenHeart = 10051,      //100051 ~ 100055 
        WindCutter = 10061,       //100061 ~ 100065 
        EgoSword = 10071,         //100071 ~ 100075 
        MonsterSkill_01 = 10091,
        BasicAttack = 100101,
        Charging = 100201,
        Dash = 100301,
        WarCry = 100401,
        StormBlade = 100501,

    }

    public enum CreatureType
    {
        None,
        Player,
        Monster,
        RegularMonster,
        Boss
    }
    
    public enum MineralType
    {
        Stone,
    }

    public enum Polymorph
    {
        BlueSlime,
        Goblin,
        Snake,
        GoblinLoad,
    }

    public enum Scene
    {
        Unknown,
        TitleScene,
        LobbyScene,
        GameScene,
        UnderGroundScene,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Preseed,
        PointerDown,
        PointerUp,
        BeginDrag,
        Drag,
        EndDrag,
    }
    #endregion

}
static class Constants
{
    public const int StartTokenLayerNum = 1000;
    public const int StartMouseTokenLayerNum = 1050;
    public const int MaxStackTokenNum = 30;
    public const double Pi = 3.141592f;

    public const int ExceptNothing = 0;
    public const int ExceptBlankTokenContoller = 1;
    public const int ExceptNatureTokencontroller = 2;
    public const int ExceptFactoryTokenContoller = 4;
    public const int ExceptMaterialTokenContoller = 8;

}

// public static class EquipmentUIColors
// {
//     #region 장비 이름 색상
//     public static readonly Color CommonNameColor = HexToColor("A2A2A2");
//     public static readonly Color UncommonNameColor = HexToColor("57FF0B");
//     public static readonly Color RareNameColor = HexToColor("2471E0");
//     public static readonly Color EpicNameColor = HexToColor("9F37F2");
//     public static readonly Color LegendaryNameColor = HexToColor("F67B09");
//     public static readonly Color MythNameColor = HexToColor("F1331A");
//     #endregion
//     #region 테두리 색상
//     public static readonly Color Common = HexToColor("AC9B83");
//     public static readonly Color Uncommon = HexToColor("73EC4E");
//     public static readonly Color Rare = HexToColor("0F84FF");
//     public static readonly Color Epic = HexToColor("B740EA");
//     public static readonly Color Legendary = HexToColor("F19B02");
//     public static readonly Color Myth = HexToColor("FC2302");
//     #endregion
//     #region 배경색상
//     public static readonly Color EpicBg = HexToColor("#9F37F2");
//     public static readonly Color LegendaryBg = HexToColor("#F67B09");
//     public static readonly Color MythBg = HexToColor("#F1331A");
//     #endregion
// }
