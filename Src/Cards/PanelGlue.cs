using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Character;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaManBattleNetwork.Src.Powers;
using BaseLib.Extensions;
using System.Buffers;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class PanelGlue(): CardAbstract(1,CardType.Skill,CardRarity.Common,TargetType.Self,true)
{
    public string cardID = "panel_glue";

    //卡图，如果没有就注释掉
    //cardId
    //卡图为止为$"{cardImgPathRoot}/.png"
    public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";    //卡图  

    //卡牌内登记的变量，用于被json文件读取。      
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<WaterStylePower>(5),
        new PowerVar<PanelsTaken>(1),
        new CardsVar(1)
    ];

    //卡牌的关键字
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [];

    //卡牌的额外提示
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<WaterStylePower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        //卡牌打出的效果逻辑
        await PowerCmd.Apply<WaterStylePower>(Owner.Creature,DynamicVars.Power<WaterStylePower>().IntValue,Owner.Creature,this);
        await PowerCmd.Apply<PanelsTaken>(Owner.Creature,DynamicVars.Power<PanelsTaken>().IntValue,Owner.Creature,this);
        await CardPileCmd.Draw(choiceContext,DynamicVars.Cards.BaseValue,Owner);
	}

    protected override void OnUpgrade()
    {
        //升级后改变哪些东西
        DynamicVars.Power<PanelsTaken>().UpgradeValueBy(1);
    }
}