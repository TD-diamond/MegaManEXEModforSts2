using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaManBattleNetwork.Src.Character;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class BugFix() : CardAbstract(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
{
    public string cardID = "BugFix";
    public override string PortraitPath => $"{cardImgPathRoot}/bug_fix.png";

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature,"cast",0.0f);
        var debuffPowers = Owner.Creature.Powers
            .Where(power => power.Type == PowerType.Debuff)
            .ToList();

        foreach (var debuffPower in debuffPowers)
        {
            await PowerCmd.Remove(debuffPower);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
