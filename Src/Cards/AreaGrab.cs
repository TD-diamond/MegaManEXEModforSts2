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
public class AreaGrab() : CardAbstract(1,CardType.Skill,CardRarity.Common,TargetType.Self,true)
{
    //TODO:添加占位效果

    public string cardID = "AreaGrab";
    public override string PortraitPath => $"{cardImgPathRoot}/area_grab.png";  //卡图

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(4,ValueProp.Move),
        new PowerVar<PanelsTaken>(3),
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
        .Where((PowerModel powerModel) => {return powerModel is PanelsTaken;})
        .FirstOrDefault();

        if(areaPower.Amount >= 15)
        {
            await DamageCmd.Attack(DynamicVars.Damage.IntValue)
                            .FromCard(this)
                            .TargetingAllOpponents(CombatState)
                            .Execute(choiceContext);
        }
        else
        {
            await PowerCmd.Apply<PanelsTaken>(Owner.Creature,3,Owner.Creature,this);
        }
    }
    
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}