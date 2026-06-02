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
using MegaCrit.Sts2.Core.HoverTips;
using MegaManBattleNetwork.Src.Powers;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class ChargeCustom(): CardAbstract(2,CardType.Power,CardRarity.Uncommon,TargetType.Self,true)
{
    public string cardID = "charge_custom";

    //卡图，如果没有就注释掉
    //cardId
    //卡图为止为$"{cardImgPathRoot}/.png"
    // public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";    //卡图  

    //卡牌内登记的变量，用于被json文件读取。      
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1),
        new DamageVar(10,ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(Owner.Creature,"cast",0.0f);
        //卡牌打出的效果逻辑
        List<CardModel> cardsIn = PileType.Draw.GetPile(Owner).Cards
                                    .Where((CardModel card) => 
                                    {
                                        return card.EnergyCost.Canonical == 1 && card.Type == CardType.Attack
                                        && (card.Rarity == CardRarity.Common||card.Rarity == CardRarity.Uncommon);
                                    }).ToList();
        if(cardsIn.Count == 0)
            return;
        CardModel card =( 
            await CardSelectCmd.FromSimpleGrid(
                context:choiceContext,
                cardsIn:cardsIn,
                prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1),
                player: Owner)).FirstOrDefault();

        await PowerCmd.Apply<ChargeCustomPower>(Owner.Creature,1,Owner.Creature,this);
        ChargeCustomPower onHitPower = Owner.Creature.Powers.OfType<ChargeCustomPower>().FirstOrDefault();
            onHitPower?.SetCard(card);

	}

    protected override void OnUpgrade()
    {
        //升级后改变哪些东西
        EnergyCost.UpgradeBy(-1);
    }
}
