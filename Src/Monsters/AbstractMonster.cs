using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib;
using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;


namespace MegaManBattleNetwork.Src.Monsters;

public abstract class AbstractMonster : CustomMonsterModel
{
    public AbstractMonster(int minInitialHpLA,int minInitialHpHA,
        int maxInitialHpLA,int maxInitialHpHA)
    {
        minInitialHp = AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, minInitialHpHA, minInitialHpLA);
        maxInitialHp = AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, maxInitialHpHA, maxInitialHpLA);
    }
    private int minInitialHp;
    private int maxInitialHp;
    public override int MinInitialHp => minInitialHp;
    public override int MaxInitialHp => maxInitialHp;

    public override NCreatureVisuals CreateCustomVisuals() => NodeFactory<NCreatureVisuals>.CreateFromScene("res://test/scenes/test_monster.tscn");

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        return null;
    }


}