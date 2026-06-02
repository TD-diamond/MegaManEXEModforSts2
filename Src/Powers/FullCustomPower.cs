using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;

namespace MegaManBattleNetwork.Src.Powers;

public class FullCustomPower():PowerAbstracts(PowerType.Buff,PowerStackType.Single)
{
    public override string CustomPackedIconPath => $"{customIconRoot}/full_custom_power.png";
    public override string CustomBigIconPath => $"{customIconRoot}/full_custom_power.png";

    public override bool ShouldTakeExtraTurn(Player player)
    {
        return true;
    }

    public override async Task AfterTakingExtraTurn(Player player)
    {
        await PowerCmd.Remove<FullCustomPower>(player.Creature);
    }
}