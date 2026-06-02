using System.Threading.Tasks;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;

namespace MegaManBattleNetwork.Src.Powers;

public class PowersCostUp():PowerAbstracts(PowerType.Debuff,PowerStackType.Counter)
{
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side)
        {
            return;
        }
        await PowerCmd.Remove(this);
    }

    public override Task AfterRemoved(Creature oldOwner)
    {
        return base.AfterRemoved(oldOwner);
    }
    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        if (card.Owner.Creature != Owner || card.Type != CardType.Power)
        {
            modifiedCost = originalCost;
            return false;
        }
        if(card.EnergyCost.Canonical >= 3)
        {
            modifiedCost = originalCost;
            return false;
        }
        modifiedCost = 3;
        return true;
    }
}