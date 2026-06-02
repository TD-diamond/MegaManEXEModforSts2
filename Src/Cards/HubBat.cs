using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaManBattleNetwork.Src.Character;
using MegaManBattleNetwork.Src.Powers;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class HubBat() : CardAbstract(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
{
    public string cardID = "hub_bat";
    public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png"; 

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(7),
        new EnergyVar(1),
        new PowerVar<FloatShoesPower>(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<FloatShoesPower>(),
        HoverTipFactory.FromPower<Distance>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature,"cast",0.0f);
        await PowerCmd.Apply<HubBatHandDrawPower>(Owner.Creature, DynamicVars.Cards.IntValue, Owner.Creature, this);
        await PowerCmd.Apply<HubBatEnergyPower>(Owner.Creature, DynamicVars.Energy.IntValue, Owner.Creature, this);
        await PowerCmd.Apply<FloatShoesPower>(Owner.Creature, DynamicVars.Power<FloatShoesPower>().IntValue, Owner.Creature, this);

        decimal maxHpLoss = decimal.Ceiling(Owner.Creature.MaxHp * 0.2m);
        maxHpLoss = Math.Max(1m, maxHpLoss);
        await CreatureCmd.LoseMaxHp(choiceContext, Owner.Creature, maxHpLoss, true);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}
