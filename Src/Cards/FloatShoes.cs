using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaManBattleNetwork.Src.Character;
using MegaManBattleNetwork.Src.Powers;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class FloatShoes() : CardAbstract(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
{
    public string cardID = "float_shoes";

    public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<FloatShoesPower>(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<FloatShoesPower>(),
        HoverTipFactory.FromPower<Distance>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature,"cast",0.0f);
        await PowerCmd.Apply<FloatShoesPower>(Owner.Creature, DynamicVars.Power<FloatShoesPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
