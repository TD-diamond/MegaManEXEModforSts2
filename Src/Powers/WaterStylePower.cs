using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;

namespace MegaManBattleNetwork.Src.Powers;

public class WaterStylePower():PowerAbstracts(PowerType.Debuff,PowerStackType.Counter)
{
    public override string CustomPackedIconPath => $"{customIconRoot}/water_style_power.png";
    public override string CustomBigIconPath => $"{customIconRoot}/water_style_power.png";

    public override async Task AfterApplied(Creature applier, CardModel cardSource)
    {
        if(Owner.HasPower<GrassStylePower>())
            await PowerCmd.Remove<GrassStylePower>(Owner);
        if(Owner.HasPower<ElecStylePower>())
            await PowerCmd.Remove<ElecStylePower>(Owner);
        //克制属�?
        if(Owner.HasPower<FireStylePower>())
        {
            await PowerCmd.Remove<FireStylePower>(Owner);
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner, Amount, ValueProp.Unpowered, null, null);
        }
        lockedAmount = Amount;
        isLocked = true;
    }
    public override async Task BeforeApplied(Creature target, decimal amount, Creature applier, CardModel cardSource)
    {
        if (target.HasPower<WaterStylePower>())
            await PowerCmd.Remove<WaterStylePower>(target);
    }

    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature applier, CardModel cardSource)
    {
        if (isRestoring || !isLocked) return;
        if (power != this || power.Owner != Owner) return;

        if (Amount != lockedAmount)
        {
            isRestoring = true;
            await PowerCmd.SetAmount<WaterStylePower>(Owner, lockedAmount, Owner, null);
            isRestoring = false;
        }
    }

    public override Task AfterRemoved(Creature oldOwner)
    {
        isLocked = false;
        lockedAmount = 0;
        return Task.CompletedTask;
    }

    private decimal lockedAmount;
    private bool isLocked;
    private bool isRestoring;
}
