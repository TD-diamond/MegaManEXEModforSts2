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
using MegaManBattleNetwork.Src.Powers;
using MegaCrit.Sts2.Core.Models.Cards;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class BusterUp():CardAbstract(1,CardType.Power,CardRarity.Uncommon,TargetType.Self,true)
{
    public string cardID = "BusterUp";
    public override string PortraitPath => $"{cardImgPathRoot}/buster_up.png";    //卡图  
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<MegaBuster>()];
                 
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new PowerVar<BusterUpPower>(3),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature,"cast",0.0f);
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast",Owner.Character.CastAnimDelay);
        int powerAmount = Owner.Creature.GetPowerAmount<BusterUpPower>();
        powerAmount = powerAmount + DynamicVars.Power<BusterUpPower>().IntValue;
        await PowerCmd.Remove<BusterUpPower>(Owner.Creature);
        await PowerCmd.Apply<BusterUpPower>(Owner.Creature,powerAmount,Owner.Creature,this);
        if(IsUpgraded)
        {
            CardModel newBuster = CombatState.CreateCard<MegaBuster>(Owner);
            await CardPileCmd.Add(newBuster,PileType.Hand);
        }
    }

    protected override void OnUpgrade()
    {
        base.OnUpgrade();
    }
}
