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

public class BusterUpPower():PowerAbstracts(PowerType.Buff,PowerStackType.Counter)
{
    public override string CustomPackedIconPath => $"{customIconRoot}/buster_up_power.png";
    public override string CustomBigIconPath => $"{customIconRoot}/buster_up_power.png";

    public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel card)
    {
        if (Owner != dealer)
        {
            return 0m;
        }

        if (!props.IsPoweredAttack())
        {
            return 0m;
        }

        if (card == null)
        {
            return 0m;
        }

        if (!card.Tags.Contains((CardTag)CustomCardTag.Buster))
        {
            return 0m;
        }

        return Amount;
    }
}