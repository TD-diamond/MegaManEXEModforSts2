using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaManBattleNetwork.Src.Character;
using MegaManBattleNetwork.Src.Powers;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class ShieldCustom() : CardAbstract(2, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
{
    public string cardID = "shield_custom";
    // public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png"; 

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<CardModel> cardsIn = PileType.Draw.GetPile(Owner).Cards
            .Where(card =>
            {
                    if(card is not CardAbstract)
                        return false;
                    CardAbstract cd = card as CardAbstract;
                    return cd.EnergyCost.Canonical == 1 
                    && cd.Type == CardType.Skill
                    && cd.GetVars("Block") != null;
            })
            .ToList();

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

        await CreatureCmd.TriggerAnim(Owner.Creature,"shield_equipped",0.0f);
        await PowerCmd.Apply<ChargeCustomPower>(Owner.Creature, 1, Owner.Creature, this);
        ChargeCustomPower shieldCustomPower = Owner.Creature.Powers.OfType<ChargeCustomPower>().FirstOrDefault();
        shieldCustomPower?.SetCard(selectedCard);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
