using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaManBattleNetwork.Src.Character;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class AccurateSearch() : CardAbstract(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
{
    public string cardID = "accurate_search";

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<CardModel> cardsIn = PileType.Draw.GetPile(Owner).Cards.ToList();
        if (cardsIn.Count == 0)
        {
            return;
        }

        CardModel selectedCard = (
            await CardSelectCmd.FromSimpleGrid(
                context: choiceContext,
                cardsIn: cardsIn,
                prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1),
                player: Owner)).FirstOrDefault();

        if (selectedCard == null)
        {
            return;
        }

        await CardPileCmd.Add(selectedCard, PileType.Hand);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
