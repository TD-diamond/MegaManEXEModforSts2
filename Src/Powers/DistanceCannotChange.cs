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

public class DistanceCannotChange():PowerAbstracts(PowerType.Debuff,PowerStackType.Counter)
{
    // public override string CustomPackedIconPath => $"{customIconRoot}/areas.png";
    // public override string CustomBigIconPath => $"{customIconRoot}/areas.png";
    public override async Task AfterApplied(Creature applier, CardModel cardSource)
    {
        bool flag = Owner.HasPower<Distance>(); 
        skip = true;
        await PowerCmd.Remove<Distance>(Owner);
        await PowerCmd.Apply<Distance>(Owner,Amount,Owner,null);
    }


    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if(side == Owner.Side)
            return;
        skip = false;

        if(!Owner.HasPower<Distance>())
            await PowerCmd.Apply<Distance>(Owner,1,Owner,null);

        await PowerCmd.Remove<DistanceCannotChange>(Owner);
    }

    public override Task AfterRemoved(Creature oldOwner)
    {
        List<PowerModel> list = Owner.Powers.Where((PowerModel powerModel) =>{return powerModel is NegativePanels;}).ToList();
        int decreaseAmount = 0;
        foreach(PowerModel pm in list){
            decreaseAmount += pm.Amount;
        }
        int amount = Owner.GetPower<PanelsTaken>().Amount/3 - decreaseAmount/3 - 3;
        PowerCmd.Remove<Distance>(Owner);
        PowerCmd.Apply<Distance>(Owner,amount,Owner,null); 
        return Task.CompletedTask;
    }
    

    public bool Skip {get{return skip;}}
    protected bool skip = false;
}