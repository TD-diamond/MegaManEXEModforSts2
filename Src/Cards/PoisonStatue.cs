using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaManBattleNetwork.Src.Character;
using MegaManBattleNetwork.Src.Cmd;
using MegaManBattleNetwork.Src.Orbs;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class PoisonStatue() : CardAbstract(2, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
{
    public string cardID = "poison_statue";
    public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HealVar(20)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<PoisonPower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature,"cast",0.0f);
        await BuildCmd.Summon<PoisonAnubisBuild>(choiceContext,Owner,this);
        Creature anubis = BuildCmd.GetBuild(Owner);
        await BuildCmd.SetBuildMaxHp(anubis,DynamicVars.Heal.IntValue);
        await BuildCmd.HealBuild(anubis,999);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(5);
    }

}
