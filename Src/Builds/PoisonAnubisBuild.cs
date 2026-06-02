
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils.NodeFactories;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaManBattleNetwork.Src.Builds;

public class PoisonAnubisBuild:AbstractBuild
{
    public override int MinInitialHp => 20;
    public override int MaxInitialHp => 20;

     public override NCreatureVisuals CreateCustomVisuals()
        => NodeFactory<NCreatureVisuals>
            .CreateFromScene("res://MegaManBattleNetwork/Scenes/builds/PoisonAnubis.tscn");

    int poisonValue = 5;

    public int PoisonValue
    {
        set{poisonValue = PoisonValue;}
        get{return poisonValue;}
    }
    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
    {
        if(side != Creature.PetOwner.Creature.Side)
            return Task.CompletedTask;
        return ApplyPoison();
    }
    
    private async Task ApplyPoison()
    {
        foreach (Creature enemy in CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<PoisonPower>(enemy, poisonValue, Creature.PetOwner.Creature, null);
        }
    }
}