using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;

namespace MegaManBattleNetwork.Src.Powers;

public class ChargeCustomPower() : PowerAbstracts(PowerType.Buff,PowerStackType.Single)
{
    public override string CustomPackedIconPath => $"{customIconRoot}/null_power.png";
    public override string CustomBigIconPath => $"{customIconRoot}/null_power.png";

    public override Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        if(cardToCopy == null)
        {
            return Task.CompletedTask;
        }
        if(player.Creature != Owner)
        {
            return Task.CompletedTask;
        }
        CardModel card = cardToCopy.CreateClone();
            card.AddKeyword(CardKeyword.Exhaust);
            card.AddKeyword(CardKeyword.Ethereal);
        return CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, addedByPlayer: true);
    }
    public void SetCard(CardModel card)
    {
        cardToCopy = card.CreateClone();
    }

    private CardModel cardToCopy = null;
}