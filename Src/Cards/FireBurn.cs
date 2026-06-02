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
using BaseLib.Extensions;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class FireBurn(): CardAbstract(1,CardType.Skill,CardRarity.Uncommon,TargetType.AllEnemies,true)
{
    public string cardID = "fire_burn";

    public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";    //卡图  

    //卡牌内登记的变量，用于被json文件读取。      
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8,ValueProp.Move),
        new PowerVar<BrokenPanels>(2)
    ];


    //卡牌的额外提示
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<FireStylePower>(),
        HoverTipFactory.FromPower<BrokenPanels>(),
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await ToAttackWithStyle(choiceContext,cardPlay,
                    PowerTypes.Fire,async ()=>
                    {
                        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                        .FromCard(this)
                        .TargetingAllOpponents(CombatState)
                        .Execute(choiceContext);
                        await PowerCmd.Apply<BrokenPanels>(Owner.Creature,DynamicVars.Power<BrokenPanels>().IntValue,Owner.Creature,this);
                    });
	}

    protected override void OnUpgrade()
    {
        //升级后改变哪些东西
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars.Power<BrokenPanels>().UpgradeValueBy(1);
    }
}