using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Character;
using MegaManBattleNetwork.Src.Powers;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class Invisible() : CardAbstract(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
{
    public string cardID = "invisible";

    public override string PortraitPath => $"{cardImgPathRoot}/invisible.png"; 

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new PowerVar<IntangiblePower>(1),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => 
    [
        CardKeyword.Ethereal,
        CardKeyword.Exhaust
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<IntangiblePower>(),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature,"cast",0.0f);
        await PowerCmd.Apply<IntangiblePower>(Owner.Creature,DynamicVars.Power<IntangiblePower>().IntValue,Owner.Creature,this);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Ethereal);
    }
}
