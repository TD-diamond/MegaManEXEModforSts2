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

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class CustomToolkit(): CardAbstract(0,CardType.Power,CardRarity.Rare,TargetType.Self,true)
{
    public string cardID = "custom_toolkit";

    //卡图，如果没有就注释掉
    //cardId
    //卡图为止为$"{cardImgPathRoot}/.png"
    // public override string PortraitPath => $"{cardImgPathRoot}/add_button.png";    //卡图  

    //卡牌内登记的变量，用于被json文件读取。      
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3),
        new EnergyVar(2),
        new PowerVar<PowersCostUp>(1),
        new PowerVar<MachineLearningPower>(1)
    ];

    //卡牌的关键字
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    //卡牌的额外提示
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>[];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(Owner.Creature,"cast",0.0f);
        //卡牌打出的效果逻辑
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue,Owner);
        await CardPileCmd.Draw(choiceContext,DynamicVars.Cards.BaseValue,Owner);
        await PowerCmd.Apply<MachineLearningPower>(Owner.Creature,DynamicVars.Power<MachineLearningPower>().BaseValue,Owner.Creature,this);
        await PowerCmd.Apply<PowersCostUp>(Owner.Creature,DynamicVars.Power<PowersCostUp>().BaseValue,Owner.Creature,this);
	}

    protected override void OnUpgrade()
    {
        //升级后改变哪些东西
        DynamicVars.Cards.UpgradeValueBy(2);
    }
}
