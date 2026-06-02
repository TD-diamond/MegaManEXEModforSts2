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
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;

namespace MegaManBattleNetwork.Src.Powers;

public class PanelsTaken() : PowerAbstracts(PowerType.Buff,PowerStackType.Counter)
{
    public override string CustomPackedIconPath => $"{customIconRoot}/panels_taken.png";
    public override string CustomBigIconPath => $"{customIconRoot}/panels_taken.png";
    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (isRestoring) return Task.CompletedTask;
        if(Owner != Owner) return Task.CompletedTask;

        isRestoring = true;
        if (Amount > 15 || Amount < 3)
        {
            PowerCmd.SetAmount<PanelsTaken>(Owner, (Amount > 15)?15:3, Owner, null);
        }
        bool canChange = Owner.Powers.Where((PowerModel powerModel)=>
            {return powerModel is DistanceCannotChange;}).ToList().Count == 0;
        if(canChange)
            ChangeDistance();
        isRestoring = false;
        return Task.CompletedTask;
    }
    



    private  Task ChangeDistance()
    {
        List<PowerModel> list = Owner.Powers.Where((PowerModel powerModel) =>{return powerModel is NegativePanels;}).ToList();
        int decreaseAmount = 0;
        foreach(PowerModel pm in list){
            decreaseAmount += pm.Amount;
        }
        int amount = Amount/3 - decreaseAmount/3 - 3;
        PowerCmd.Remove<Distance>(Owner);
        PowerCmd.Apply<Distance>(Owner,amount,Owner,null); 
        return Task.CompletedTask;
    }

    public override async Task AfterRemoved(Creature oldOwner)
    {
        await PowerCmd.SetAmount<PanelsTaken>(Owner,3,Owner,null);
    }

    private bool isRestoring = false;
}