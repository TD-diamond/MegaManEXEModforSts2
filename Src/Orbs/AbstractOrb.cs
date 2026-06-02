using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaManBattleNetwork.Src.Orbs;

public abstract class AbstractOrb : CustomOrbModel
{
    public AbstractOrb(decimal passive, decimal evoke = 0)
    {
        passiveVal = passive;
        evokeVal = evoke;
    }
    // 被动效果数值，使用ModifyOrbValue的数值将会吃到集中的加成。
    // 如果不想被集中干扰，则直接赋予数值即可。

    protected string rootPathImg = $"res://MegaManBattleNetwork/Images/Orbs/";
    protected string rootPathScene = $"res://MegaManBattleNetwork/Scenes/Orbs/";
    protected decimal passiveVal = 0;
    protected decimal evokeVal = 0;
    public override decimal PassiveVal => passiveVal;

    // 激发效果数值
    public override decimal EvokeVal => evokeVal;

    // 暗色，使用球的主体色的暗色调
    public override Color DarkenedColor => new (0.1f,0.2f,0.5f);

    //出现在随机球池中
    public override bool IncludeInRandomPool => false;

    // 提示图标路径
    public override string CustomIconPath => $"{rootPathImg}stone_cube_orb.png";

    public override decimal ModifyHpLostAfterOstyLate(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
    {
        if(target != Owner.Creature)
            return amount;
        decimal resultDamage = amount - passiveVal + 1;
        GD.Print(resultDamage);
        passiveVal = Math.Max(0m,passiveVal - amount + 1);
        return resultDamage > 1?resultDamage:1;
    }

    public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature dealer, CardModel cardSource)
    {
        if(target != Owner.Creature)
            return Task.CompletedTask;

        return (passiveVal <= 0)?Evoke(choiceContext):Task.CompletedTask;
    }

    public override Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        return passiveVal == 0?Evoke(choiceContext):Task.CompletedTask;
    }

    
    // 球的场景的路径。如果你使用这个，你必须要有一个名称为SpineSkeleton并且是SpineSprite类型的节点
    // public override string? CustomSpritePath => "res://test/scenes/test_orb.tscn";

    // 可以继承这个并自行搭建场景，只需父节点是Node2D即可。这样就没有上述限制。代码上优先使用这个
    public override Node2D CreateCustomSprite()
    {
        return PreloadManager.Cache.GetScene($"{rootPathScene}stone_cube_orb.tscn").Instantiate<Node2D>();
    }

    // 触发被动
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature target)
    {
        Trigger();
    }

    // 触发激发，返回受影响的角色
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        return [Owner.Creature];
    }
}
