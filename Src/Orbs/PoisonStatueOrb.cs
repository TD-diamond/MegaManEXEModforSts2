using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaManBattleNetwork.Src.Orbs;

public class PoisonStatueOrb() : AbstractOrb(20, 0)
{
    public override Color DarkenedColor => new(0.25f, 0.55f, 0.2f);
    public override string CustomIconPath => $"{rootPathImg}poison_statue.png";
    public override Node2D CreateCustomSprite()
    {
        return PreloadManager.Cache.GetScene($"{rootPathScene}poison_statue_orb.tscn").Instantiate<Node2D>();
    }

    public override Task AfterTurnStartOrbTrigger(PlayerChoiceContext choiceContext)
    {
        return Passive(choiceContext,null);
    }


    public override async Task Passive(PlayerChoiceContext choiceContext, Creature target)
    {
        Trigger();
        foreach (Creature enemy in CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<PoisonPower>(enemy, 5, Owner.Creature, null);
        }
    }

    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        return [Owner.Creature];
    }
}
