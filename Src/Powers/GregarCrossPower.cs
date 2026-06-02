using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;

namespace MegaManBattleNetwork.Src.Powers;

public class GregarCrossPower() : PowerAbstracts(PowerType.Buff, PowerStackType.Counter)
{
    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        CardModel card = CombatState.CreateCard<GregarSkill>(Owner.Player);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, true);
    }

    public override async Task AfterApplied(Creature applier, CardModel cardSource)
    {
        await PowerCmd.Apply<RapidShootingPower>(Owner,2,Owner,null);
        await PowerCmd.Apply<StrengthPower>(Owner,1,Owner,null);
    }
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side)
        {
            return;
        }

        if (Amount <= 1)
        {
            await PowerCmd.Remove<BeastCrossDemisePower>(Owner);
            await PowerCmd.SetAmount<RapidShootingPower>(Owner,Owner.GetPower<RapidShootingPower>().Amount - 2,Owner,null);
        }

        await PowerCmd.Decrement(this);
    }

}
