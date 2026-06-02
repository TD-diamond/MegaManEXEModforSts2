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
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class Bass():CardAbstract(0,CardType.Attack,CardRarity.Rare,TargetType.AnyEnemy,true)
{
    public string cardID = "Bass";
    public override string PortraitPath => $"{cardImgPathRoot}/bass.png";

    protected override bool HasEnergyCostX => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(6,ValueProp.Move),
        new EnergyVar(3)
    ];
    protected override bool ShouldGlowGoldInternal => 
        Owner.PlayerCombatState.Energy >= DynamicVars.Energy.IntValue && IsUpgraded;
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        int num = ResolveEnergyXValue();

        if(num >= DynamicVars.Energy.IntValue && IsUpgraded)
        {
            num *= 2;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(num)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_giant_horizontal_slash", null, "slash_attack.mp3")
            .Execute(choiceContext);

        await using AttackContext context = await AttackCommand.CreateContextAsync(CombatState, this);

        List<Creature> list = (from e in CombatState.GetTeammatesOf(cardPlay.Target)
                                where e.IsHittable && e != cardPlay.Target
                                select e).ToList();
        if (list.Count != 0)
        {
            for(int i = 0;i< num;i++)
            {                    
                AttackContext attackContext = context;
                attackContext.AddHit(await CreatureCmd.Damage(
                                            choiceContext,list, 
                                            DynamicVars.Damage.BaseValue / 2, 
                                            ValueProp.Unpowered | ValueProp.Move,
                                            Owner.Creature, this));
            }
        }        
    }

    protected override void OnUpgrade()
    {  
        base.OnUpgrade();
    }

}
