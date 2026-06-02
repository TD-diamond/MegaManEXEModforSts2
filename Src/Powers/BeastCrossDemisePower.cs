using System.Formats.Asn1;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;

namespace MegaManBattleNetwork.Src.Powers;

public class BeastCrossDemisePower():PowerAbstracts(PowerType.Debuff,PowerStackType.Counter)
{
    public override async Task AfterRemoved(Creature oldOwner)
    {
        await PowerCmd.Apply<DemisePower>(Owner,Amount,Owner,null);
        await PowerCmd.Remove<DistanceCannotChange>(Owner);
    }

    public override async Task AfterApplied(Creature applier, CardModel cardSource)
    {
        await PowerCmd.Apply<DistanceCannotChange>(Owner,2,Owner,null);
    }

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
    {
        if(side != Owner.Side)
            return;

        await PowerCmd.Apply<DistanceCannotChange>(Owner,2,Owner,null);
    }
}