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

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class DoubleThrow(): BrokenPanelCards(1,2,CardType.Attack,CardRarity.Common,TargetType.AnyEnemy,true)
{
    public string cardID = "double_throw";

    //卡图，如果没有就注释掉
    //cardId
    //卡图为止为$"{cardImgPathRoot}/.png"
    public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";    //卡图  

    //卡牌内登记的变量，用于被json文件读取。      
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    base.CanonicalVars.Concat(
    [
        new DamageVar(5,ValueProp.Move),
        new RepeatVar(2),
    ]);

    //卡牌的关键字
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override HashSet<CardTag> CanonicalTags =>
    [
        (CardTag)CustomCardTag.PanelThrowing
    ];

    //卡牌的额外提示
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    base.ExtraHoverTips;


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        //卡牌的打出的效果逻辑
        await AttackWithAnim(choiceContext,
                            ()=>
                            {
                                return DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                                        .Targeting(cardPlay.Target)
                                        .FromCard(this)
                                        .WithHitCount(DynamicVars.Repeat.IntValue);
                            });
        await base.OnPlay(choiceContext,cardPlay);
	}

    protected override void OnUpgrade()
    {
        //升级后改变哪些东西
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}