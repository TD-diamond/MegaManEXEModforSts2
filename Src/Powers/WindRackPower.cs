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
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;

namespace MegaManBattleNetwork.Src.Powers;

public class WindRackPower():DistanceCannotChange()
{
    // public override string CustomPackedIconPath => $"{customIconRoot}/areas.png";
    // public override string CustomBigIconPath => $"{customIconRoot}/areas.png";

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side){return;}

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
    {
        if(side != Owner.Side)
            return Task.CompletedTask;
        skip = false;
        if(!Owner.HasPower<Distance>())   
            PowerCmd.Apply<Distance>(Owner,Amount,Owner,null);
        else
            PowerCmd.SetAmount<Distance>(Owner,Amount,Owner,null);
        return PowerCmd.Remove<DistanceCannotChange>(Owner);
    }
}