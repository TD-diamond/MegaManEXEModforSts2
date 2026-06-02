using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaManBattleNetwork.Src.Character;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class SearchMan():CardAbstract(1,CardType.Skill,CardRarity.Uncommon,TargetType.Self,true)
{
    public string cardID = "SearchMan";
    public override string PortraitPath => $"{cardImgPathRoot}/search_man.png";    //卡图                                             
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("RetainAmount",3),
        new CardsVar(3)
    ];

    // public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        List<CardModel> list = (
            await CardSelectCmd.FromHand(
                prefs: new CardSelectorPrefs(SelectionScreenPrompt, 0, DynamicVars["RetainAmount"].IntValue), 
                context: choiceContext, player: Owner, filter:null , source: this)
                ).ToList();
        List<CardModel> returnList = PileType.Hand.GetPile(Owner).Cards.Where((CardModel card)=>
        {
           return !list.Contains(card);
        }).ToList();
        foreach (CardModel item in returnList)
		{
			await CardPileCmd.Add(item, PileType.Draw);
		}
        await CardPileCmd.ShuffleIfNecessary(choiceContext,Owner);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
	}

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}
