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
using MegaCrit.Sts2.Core.HoverTips;
using MegaManBattleNetwork.Src.Powers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models;
using System.Text.RegularExpressions;
using BaseLib.Extensions;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class RapidShootingModel() :CardAbstract(1,CardType.Power,CardRarity.Uncommon,TargetType.Self,true)
{
    public string cardID = "RapidShootingModel";
    
    // public override string PortraitPath => $"{cardImgPathRoot}/rapid_shooting_model.png";    //卡图  
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromCard<MegaBuster>(),
    ];
                 
    private string repeatTimes = "RepeatTimes";
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar(repeatTimes,1),
        new EnergyVar(2),
        new PowerVar<BusterUpPower>(2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature,"cast",0.0f);
        await PowerCmd.Apply<RapidShootingPower>(Owner.Creature,DynamicVars[repeatTimes].IntValue,Owner.Creature,this);
        int powerAmount = Owner.Creature.GetPowerAmount<BusterUpPower>();
        powerAmount = powerAmount - DynamicVars.Power<BusterUpPower>().IntValue;
        await PowerCmd.Remove<BusterUpPower>(Owner.Creature);
        await PowerCmd.Apply<BusterUpPower>(Owner.Creature,powerAmount,Owner.Creature,this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
