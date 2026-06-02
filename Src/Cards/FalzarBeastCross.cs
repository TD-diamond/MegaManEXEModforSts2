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
using MegaManBattleNetwork.Src.Cards;
using MegaManBattleNetwork.Src.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using BaseLib.Extensions;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class FalzarBeastCross():CardAbstract(2,CardType.Power,CardRarity.Rare,TargetType.Self,true)
{
    public string cardID = "FalzarBeastCross";
    public override string PortraitPath => $"{cardImgPathRoot}/falzar_beast.png";      

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new PowerVar<FalzarCrossPower>(3),
        new PowerVar<BeastCrossDemisePower>(3),
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<FalzarSkill>(),
        HoverTipFactory.FromPower<FalzarCrossPower>(),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature,"beast_cross",0.0f);
        await PowerCmd.Apply<BeastCrossDemisePower>(Owner.Creature,
                                                    DynamicVars.Power<BeastCrossDemisePower>().IntValue,
                                                    Owner.Creature,this);
        await PowerCmd.Apply<FalzarCrossPower>(Owner.Creature,
                                                DynamicVars["FalzarCrossPower"].IntValue,
                                                Owner.Creature,this);
    }
    
    protected override void OnUpgrade()
    {
       DynamicVars.Power<BeastCrossDemisePower>().UpgradeValueBy(-1);
    }
}
