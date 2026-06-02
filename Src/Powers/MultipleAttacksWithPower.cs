using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;

namespace MegaManBattleNetwork.Src.Powers;
public enum PowerTypes
    {
        None,
        Fire,
        Grass,
        Elec,
        Water,
        Sword,
        Float,
        Aim,
        Break,
    }
public class MultipleAttacksWithPower():PowerAbstracts(PowerType.Buff,PowerStackType.Single)
{
    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature dealer, DamageResult result, ValueProp props, Creature target, CardModel cardSource)
    {
        switch(powerType)
        {
            case (PowerTypes.Fire):
            {
                await PowerCmd.Apply<FireStylePower>(target,val,dealer,cardSource);
                break;
            }
            case (PowerTypes.Grass):
            {
                await PowerCmd.Apply<GrassStylePower>(target,val,dealer,cardSource);
                break;
            }
            case (PowerTypes.Elec):
            {
                await PowerCmd.Apply<ElecStylePower>(target,val,dealer,cardSource);
                break;
            }
            case (PowerTypes.Water):
            {
                await PowerCmd.Apply<WaterStylePower>(target,val,dealer,cardSource);
                break;
            }
            case (PowerTypes.Aim):
            {
                await PowerCmd.Apply<AimStylePower>(target,val,dealer,cardSource);
                break;
            }
            case (PowerTypes.Break):
            {
                await PowerCmd.Apply<BreakStylePower>(target,val,dealer,cardSource);
                break;
            }
            case (PowerTypes.Sword):
            {
                await PowerCmd.Apply<SwordStylePower>(target,val,dealer,cardSource);
                break;
            }
            case (PowerTypes.Float):
            {
                await PowerCmd.Apply<FloatStylePower>(target,val,dealer,cardSource);
                break;
            }
            case(PowerTypes.None):
                break;
            default:
                break;
        }
    }

    public void SetPower(PowerTypes types,int val)
    {
        powerType = types;
        this.val = val;
    }

    

    private PowerTypes powerType = PowerTypes.None;
    private int val = 1;
}