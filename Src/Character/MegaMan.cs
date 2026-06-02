using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegaManBattleNetwork.Src.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaManBattleNetwork.Src.Relics;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaManBattleNetwork.Src.Character;

public class MegaMan : PlaceholderCharacterModel
{
    public const string CharacterId = "MegaMan";                         //角色ID

    public static readonly Color Color = new("00BFFF");                     //深青色

    public override Color NameColor => Color;                               //名称颜色

    public override Color MapDrawingColor => Color;                         //地图绘制颜色
    
    public override CharacterGender Gender => CharacterGender.Masculine;    //角色性别
    public override int StartingHp => 80;                                   //血量

    protected static string ScenePathRoot = $"res://MegaManBattleNetwork/Scenes";
    protected static string ImgPathRoot = $"res://MegaManBattleNetwork/Images";
    protected static string characterFolder = $"Character";
    protected string ScenePathCharacterRoot = $"{ScenePathRoot}/{characterFolder}";
    protected string ImgPathCharacterRoot = $"{ImgPathRoot}/{characterFolder}";
    //人物模型
    public override string CustomVisualPath => $"{ScenePathRoot}/Character/mega_man_animation.tscn"; 

    // 人物头像路径。
    public override string CustomIconTexturePath => $"{ImgPathCharacterRoot}/icon/megaman_icon.png";

    // 能量表盘tscn路径。
    // public override string CustomEnergyCounterPath => $"{ScenePathCharacterRoot}/megaman_energy_counter.tscn";

    // 人物选择图标。
    public override string CustomCharacterSelectIconPath => $"{ImgPathCharacterRoot}/selectIcon/MegaManSelectIcon.png";
    // 人物选择图标-锁定状态。
    public override string CustomCharacterSelectLockedIconPath => $"{ImgPathCharacterRoot}/selectIcon/MegaManLocked.png";

    //初始牌组
    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<MegaManStrike>(),
        ModelDb.Card<MegaManStrike>(),
        ModelDb.Card<MegaManStrike>(),
        ModelDb.Card<MegaManStrike>(),
        ModelDb.Card<MegaManDefend>(),
        ModelDb.Card<MegaManDefend>(),
        ModelDb.Card<MegaManDefend>(),
        ModelDb.Card<MegaManDefend>(),
        ModelDb.Card<MegaBuster>(),
        ModelDb.Card<PanelThrow>(),
        ModelDb.Card<PanelGrab>(),
    ];

    //初始遗物
    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<Pet>()
    ];

    //角色池
    public override CardPoolModel CardPool => ModelDb.CardPool<MegaManCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<MegaManRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<MegaManPotionPool>();

}