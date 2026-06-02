using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using BaseLib;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Builds;
using MegaManBattleNetwork.Src.Powers;


namespace MegaManBattleNetwork.Src.Cmd;
public static class BuildCmd
{
    public static async Task<SummonResult> Summon<T>(PlayerChoiceContext choiceContext, Player summoner, AbstractModel source)where T : AbstractBuild
    {
		Creature pet = summoner.Creature.Pets.FirstOrDefault((Creature c) => c.Monster is AbstractBuild && c.PetOwner == summoner);
		//这段应该会在获得其他事件宠物之后出问题，
		
		if(pet != null)
		{
			await CreatureCmd.Kill(pet);
		}

        CombatState combatState = summoner.Creature.CombatState;

        if (CombatManager.Instance.IsInProgress)
        {
            SfxCmd.Play("event:/sfx/characters/necrobinder/necrobinder_summon");
        }

        Creature build = combatState.Allies.FirstOrDefault((Creature c) => c.Monster is T && c.PetOwner == summoner);
        
        {

            
			build = await PlayerCmd.AddPet<T>(summoner);
			NCreature ostyNode = NCombatRoom.Instance?.GetCreatureNode(build);
			if (ostyNode != null && source is CardModel)
			{
				ostyNode.Modulate = Colors.Transparent;
				Tween tween = ostyNode.CreateTween();
				tween.TweenProperty(ostyNode, "modulate", Colors.White, 0.3499999940395355).SetDelay(0.10000000149011612);
				ostyNode.StartReviveAnim();
			}

			await PowerCmd.Apply<DieForYouPower>(build, 1m, null, null);
			await PowerCmd.Apply<ShowHpPower>(build,build.CurrentHp,null,null);
			ostyNode?.TrackBlockStatus(summoner.Creature);
            
        }

        if (TestMode.IsOff)
        {
            NCreature nCreature = NCombatRoom.Instance?.GetCreatureNode(build);
            nCreature.OstyScaleToSize(build.MaxHp, 0.75f);
        }

        CombatManager.Instance.History.BlockGained(combatState,summoner.Creature,build.MaxHp,ValueProp.Move,null);
        return new SummonResult(summoner.Osty, build.MaxHp);
    }

	public static Creature GetBuild(Player player)
	{
		return player.PlayerCombatState.Pets.FirstOrDefault((Creature c)=>c.Monster is AbstractBuild && c.PetOwner == player);
	}

	public static Task SetBuildMaxHp(Creature build,int val)
	{
		return CreatureCmd.SetMaxHp(build,val);
	}

	public static Task SetBuildMaxHp(Player player,int val)
	{
		Creature build = GetBuild(player);
		return CreatureCmd.SetMaxHp(build,val);
	}

	public static Task HealBuild(Creature build,int val)
	{
		return CreatureCmd.Heal(build,val);
	}
	public static Task HealBuild(Player player,int val)
	{
		Creature build = GetBuild(player);
		return CreatureCmd.Heal(build,val);
	}

}
