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
public class GrabPenalties(): CardAbstract(1,CardType.Skill,CardRarity.Uncommon,TargetType.AllEnemies,true)
{
    public string cardID = "grab_penalties";

    //卡图，如果没有就注释掉
    //cardId
    //卡图为止为$"{cardImgPathRoot}/.png"
    public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";    //卡图  

    //卡牌内登记的变量，用于被json文件读取。      
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(2,ValueProp.Move),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("PenaltyTimes").WithMultiplier((CardModel card, Creature _)
            => (9 - card.Owner.Creature.GetPowerAmount<PanelsTaken>() > 0)
            ? 9 - card.Owner.Creature.GetPowerAmount<PanelsTaken>()
            :0)
    ];

    //卡牌的关键字
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    //卡牌的额外提示
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<PanelsTaken>(),
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        //卡牌打出的效果逻辑
        int amount = Owner.Creature.GetPower<PanelsTaken>().Amount;
        int penaltyCounts = 9 - amount;
        if(penaltyCounts > 0)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                        .WithHitCount(penaltyCounts)
                        .FromCard(this)
                        .TargetingAllOpponents(CombatState)
                        .Execute(choiceContext);
            
            await PowerCmd.Apply<PanelsTaken>(Owner.Creature,penaltyCounts,Owner.Creature,this);
        }
	}

    protected override void OnUpgrade()
    {
        //升级后改变哪些东西
        DynamicVars.Damage.UpgradeValueBy(1);
    }
}