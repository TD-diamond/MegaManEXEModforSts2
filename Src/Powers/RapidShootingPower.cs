using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;

namespace MegaManBattleNetwork.Src.Powers;

public class RapidShootingPower():PowerAbstracts(PowerType.Buff,PowerStackType.Counter)
{
    public override string CustomPackedIconPath => $"{customIconRoot}/null_power.png";
    public override string CustomBigIconPath => $"{customIconRoot}/null_power.png";

    public override Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature applier, CardModel cardSource)
    {
        if (!(power is RapidShootingPower))
        {
            return Task.CompletedTask;
        }

        if (power.Owner != Owner)
        {
            return Task.CompletedTask;
        }

        IEnumerable<CardModel> enumerable = Owner.Player?.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();
        foreach (CardModel item in enumerable)
        {
            if (item is MegaBuster megaBuster)
            {
                megaBuster.SetRepeatValue((int)megaBuster.RepeatTimes + 1);
            }
        }

        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card.Owner != Owner.Player)
        {
            return Task.CompletedTask;
        }

        if (!(card is MegaBuster megaBuster))
        {
            return Task.CompletedTask;
        }

        megaBuster.SetRepeatValue(Amount + 1);
        return Task.CompletedTask;
    }

    public override Task AfterRemoved(Creature oldOwner)
    {
        IEnumerable<CardModel> enumerable = oldOwner.Player?.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();
        foreach (CardModel item in enumerable)
        {
            if (item is MegaBuster megaBuster)
            {
                megaBuster.SetRepeatValue(1);
            }
        }

        return Task.CompletedTask;
    }



}