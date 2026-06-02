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
public class HolyPanel() : CardAbstract(1,CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
{
    public string cardID = "HolyPanel";

    public override string PortraitPath => $"{cardImgPathRoot}/holy_panel.png"; 

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new PowerVar<HolyPanelPower>(1),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => 
    [
        CardKeyword.Ethereal,
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<HolyPanelPower>(),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<HolyPanelPower>(Owner.Creature,DynamicVars.Power<HolyPanelPower>().IntValue,Owner.Creature,this);
    }

   protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Ethereal);
    }
}