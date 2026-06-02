using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Powers;


namespace MegaManBattleNetwork.Src.Powers;
public class EveryTurnGainBlock():PowerAbstracts(PowerType.Buff,PowerStackType.Counter)
{
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if(side != Owner.Side)
        {
            return;
        }
        await CreatureCmd.GainBlock(Owner,Amount,ValueProp.Move,null);
    }
}