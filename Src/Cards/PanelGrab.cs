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
using MegaManBattleNetwork.Src.Powers;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class PanelGrab() : CardAbstract(1,CardType.Skill,CardRarity.Basic,TargetType.Self,true)
{
    //TODO:添加占位效果

    public string cardID = "PanelGrab";
    public override string PortraitPath => $"{cardImgPathRoot}/panel_grab.png";  //卡图

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<PanelsTaken>(1),
        new DamageVar(4,ValueProp.Move)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>    
    [
        HoverTipFactory.FromPower<PanelsTaken>(),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if(!Owner.HasPower<PanelsTaken>())
            return;
        PowerModel areaPower = Owner.Creature.Powers
        .FirstOrDefault((PowerModel powerModel) => {return powerModel is PanelsTaken;});

        if(areaPower.Amount >= 15)
        {
            await DamageCmd.Attack(DynamicVars.Damage.IntValue)
                            .FromCard(this)
                            .TargetingRandomOpponents(CombatState)
                            .Execute(choiceContext);
        }
        else
        {
            await PowerCmd.Apply<PanelsTaken>(Owner.Creature,DynamicVars.Power<PanelsTaken>().IntValue,Owner.Creature,this);
        }
    }
    
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}