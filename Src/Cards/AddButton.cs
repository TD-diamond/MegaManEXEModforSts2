using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Character;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class AddButton(): CardAbstract(1,CardType.Skill,CardRarity.Common,TargetType.Self,true)
{
    public string cardID = "AddButton";
    // public override string PortraitPath => $"{cardImgPathRoot}/add_button.png";    //卡图  

    private const string DiscardAmount = "DiscardAmount";                   
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [new DynamicVar(DiscardAmount,3)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
    
        List<CardModel> list = (
            await CardSelectCmd.FromHand(
                prefs: new CardSelectorPrefs(SelectionScreenPrompt, 0, DynamicVars[DiscardAmount].IntValue), 
                context: choiceContext, player: Owner, filter:null , source: this)
                ).ToList();
		if (list.Count == 0)
		{
			return;
		}
        foreach (CardModel item in list)
		{
			await CardPileCmd.Add(item, PileType.Discard);
            
		}
        await PowerCmd.Apply<DrawCardsNextTurnPower>(Owner.Creature,list.Count,Owner.Creature,this);
	}

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}