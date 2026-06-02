using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;

namespace MegaManBattleNetwork.Src.Powers;

public class UninstallPower():PowerAbstracts(PowerType.Buff,PowerStackType.Single)
{
    public override string CustomPackedIconPath => $"{customIconRoot}/uninstall_power.png";
    public override string CustomBigIconPath => $"{customIconRoot}/uninstall_power.png";

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if(cardPlay.Card.Type == CardType.Attack)
        {
            await PowerCmd.Remove<UninstallPower>(Owner);
        }
    }

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature dealer, DamageResult result, ValueProp props, Creature target, CardModel cardSource)
    {
        if (dealer == Owner && props.IsPoweredAttack() && result.UnblockedDamage > 0)
        {
            List<PowerModel> list = target.Powers.Where(power =>
            {
                return power.Type == PowerType.Buff;
            }).ToList();

            foreach(PowerModel p in list)
            {
                await PowerCmd.Remove(p);
            }
        }
    }
}