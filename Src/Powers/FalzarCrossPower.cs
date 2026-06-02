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

public class FalzarCrossPower():PowerAbstracts(PowerType.Buff,PowerStackType.Counter)
{

    public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
    {
        if (target != Owner)
        {
            return 1m;
        }

        if (!props.IsPoweredAttack())
        {
            return 1m;
        }

        return 0.5m;
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        CardModel card = CombatState.CreateCard<FalzarSkill>(Owner.Player); 
        await CardPileCmd.AddGeneratedCardToCombat(card,PileType.Hand,true);
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side)
        {
            return;
        }
        if(Amount <= 1)
        {
            await PowerCmd.Remove<BeastCrossDemisePower>(Owner);
        }
        
        await PowerCmd.Decrement(this);
    }
}