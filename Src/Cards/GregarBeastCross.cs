using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaManBattleNetwork.Src.Character;
using MegaManBattleNetwork.Src.Powers;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class GregarBeastCross() : CardAbstract(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
{
    public string cardID = "GregarBeastCross";
    public override string PortraitPath => $"{cardImgPathRoot}/gregar_beast.png";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<GregarCrossPower>(3),
        new PowerVar<BeastCrossDemisePower>(3),
        new PowerVar<StrengthPower>(1),
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<GregarSkill>(),
        HoverTipFactory.FromPower<GregarCrossPower>(),
        HoverTipFactory.FromCard<MegaBuster>(),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature,"beast_cross",0.0f);
        await PowerCmd.Apply<BeastCrossDemisePower>(
            Owner.Creature,
            DynamicVars.Power<BeastCrossDemisePower>().IntValue,
            Owner.Creature,this);
        await PowerCmd.Apply<GregarCrossPower>(
            Owner.Creature,
            DynamicVars.Power<BeastCrossDemisePower>().IntValue,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<BeastCrossDemisePower>().UpgradeValueBy(-1);
    }
}
