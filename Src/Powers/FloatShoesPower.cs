using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaManBattleNetwork.Src.Powers;

public class FloatShoesPower() : PowerAbstracts(PowerType.Buff, PowerStackType.Single)
{
    // public override string CustomPackedIconPath => $"{customIconRoot}/float_style_power.png";
    // public override string CustomBigIconPath => $"{customIconRoot}/float_style_power.png";

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
    {
        if (side != Owner.Side)
        {
            return;
        }

        await PowerCmd.Remove<Distance>(Owner);
        await PowerCmd.Apply<Distance>(Owner, 1, Owner, null);
    }
}
