

using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Powers;
using MegaManBattleNetwork.Src.Relics;

namespace MegaManBattleNetwork.Src.Builds;

public class AbstractBuild:CustomMonsterModel
{

     // 根据进阶提高最小血量，进阶8及以上为120，否则为100
    public override int MinInitialHp => 10;

    // 根据进阶提高最大血量，进阶8及以上为140，否则为120
    public override int MaxInitialHp => 10;
    public override bool IsHealthBarVisible => Creature.IsAlive;
    public static Vector2 MinOffset => new Vector2(150f, -75f);

    public static Vector2 MaxOffset => new Vector2(250f, -75f);

    public static Vector2 ScaleRange => new Vector2(1f, 2f);
    

    // 怪物场景，如果你的场景没有挂载脚本，参考这个
    public override NCreatureVisuals CreateCustomVisuals()
        => NodeFactory<NCreatureVisuals>
            .CreateFromScene("res://MegaManBattleNetwork/Scenes/builds/StoneCube.tscn");

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        MoveState moveState = new MoveState("NOTHING_MOVE", (IReadOnlyList<Creature> _) => Task.CompletedTask);
        moveState.FollowUpState = moveState;
        return new MonsterMoveStateMachine([moveState], moveState);
    }

    

    public override Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        if(Creature != creature)
            return Task.CompletedTask;
        
        if(creature.IsDead)
        {
            return CreatureCmd.TriggerAnim(Creature,"die",0.0f);
        }
            
        
        return PowerCmd.SetAmount<ShowHpPower>(Creature,Creature.CurrentHp,Creature,null);
    }

    public override Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature dealer, DamageResult result, ValueProp props, Creature target, CardModel cardSource)
    {
        if(Creature != Creature)
            return Task.CompletedTask;
        
        return PowerCmd.SetAmount<ShowHpPower>(Creature,Creature.CurrentHp,Creature,null);
    }
}