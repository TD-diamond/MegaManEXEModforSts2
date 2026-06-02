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
public class Snake(): CardAbstract(1,CardType.Attack,CardRarity.Uncommon,TargetType.AnyEnemy,true)
{
    public string cardID = "snake";

    public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";    //卡图  

    //卡牌内登记的变量，用于被json文件读取。      
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3,ValueProp.Move),
    ];

    //卡牌的额外提示
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<BrokenPanels>()
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        //卡牌打出的效果逻辑
        if(Owner.HasPower<BrokenPanels>())
        {
            int num = Owner.Creature.GetPowerAmount<BrokenPanels>();
            if(IsUpgraded)
                num += 3;

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(num)
                .Targeting(cardPlay.Target)
                .FromCard(this)
                .Execute(choiceContext);
            await PowerCmd.Remove<BrokenPanels>(Owner.Creature);
        }
	}
}