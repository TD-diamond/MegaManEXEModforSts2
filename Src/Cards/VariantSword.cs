using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Saves;
using MegaManBattleNetwork.Src.Character;
using MegaManBattleNetwork.Src.Powers;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class VariantSword():CardAbstract(2,CardType.Skill,CardRarity.Uncommon,TargetType.Self,true)
{
    public string cardID = "VariantSword";
    public override string PortraitPath => $"{cardImgPathRoot}/variant_sword.png";    //卡图    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>   [CardKeyword.Exhaust];
    public override IEnumerable<CardTag> Tags => [(CardTag)CustomCardTag.Sword];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>    
    [
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust),
        HoverTipFactory.FromPower<SwordStylePower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        List<CardModel> cardsIn = PileType.Draw.GetPile(Owner).Cards
                                    .Where((CardModel card) => 
                                    {
                                        return card.Tags.Contains((CardTag)CustomCardTag.Sword)
                                                && !(card is VariantSword);
                                    }
                                    ).ToList();
		CardModel cardModel = (
            await CardSelectCmd.FromSimpleGrid(
                context:choiceContext,
                cardsIn:cardsIn,
                prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1),
                player: Owner)
                ).FirstOrDefault();
		if (cardModel != null)
		{
            CardModel card = cardModel.CreateClone();
            card.AddKeyword(CardKeyword.Exhaust);
            await CardCmd.AutoPlay(choiceContext,card,null);
		}
	}

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}
