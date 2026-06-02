using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaManBattleNetwork.Src.Powers;

public class HubBatHandDrawPower() : PowerAbstracts(PowerType.Buff, PowerStackType.Counter)
{
    public override string CustomPackedIconPath => $"{customIconRoot}/hub_bat_power.png";
    public override string CustomBigIconPath => $"{customIconRoot}/hub_bat_power.png";

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (player != Owner.Player)
        {
            return count;
        }

        return Amount;
    }
}
