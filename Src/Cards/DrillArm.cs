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
public class DrillArm(): CardAbstract(1,CardType.Skill,CardRarity.Common,TargetType.AnyEnemy,true)
{
    public string cardID = "drill_arm";

    public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";    //卡图  

    //卡牌内登记的变量，用于被json文件读取。      
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8,ValueProp.Move),
        new PowerVar<DrillArmPower>(-2)
    ];

    //卡牌的关键字
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    //卡牌的额外提示
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<Distance>(),
        HoverTipFactory.FromPower<BreakStylePower>()
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        //卡牌打出的效果逻辑
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
                        .FromCard(this)
                        .Targeting(cardPlay.Target)
                        .WithAttackerAnim("drill_arm",0.0f)
                        .Execute(choiceContext);

        await PowerCmd.Apply<DrillArmPower>(Owner.Creature,DynamicVars.Power<DrillArmPower>().IntValue,Owner.Creature,this);
        await PowerCmd.Apply<BreakStylePower>(cardPlay.Target,DynamicVars.Damage.IntValue,Owner.Creature,this);
	}

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
        //升级后改变哪些东西
    }
}