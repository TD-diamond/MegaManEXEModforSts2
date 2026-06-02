using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;

namespace MegaManBattleNetwork.Src.Powers;

public class BrokenPanels():NegativePanels()
{
    public override string CustomPackedIconPath => $"{customIconRoot}/broken_panels.png";
    public override string CustomBigIconPath => $"{customIconRoot}/broken_panels.png";

    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature applier, CardModel cardSource)
    {
        turn_num = 0;
        await base.AfterPowerAmountChanged(power,amount,applier,cardSource);
    }

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
    {
        if(side != Owner.Side)
        {
            return Task.CompletedTask;
        }
        turn_num++;
        if(turn_num >= 2)
        {
            return PowerCmd.Remove<BrokenPanels>(Owner);
        }
        return Task.CompletedTask;

    }
    private int turn_num = 0;
}