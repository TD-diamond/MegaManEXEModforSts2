using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaManBattleNetwork.Src.Orbs;

public class StoneCubeOrb():AbstractOrb(10,1)
{
    // 暗色，使用球的主体色的暗色调
    public override Color DarkenedColor => new (0.1f,0.2f,0.5f);

    // 提示图标路径
    public override string CustomIconPath => $"{rootPathImg}stone_cube_orb.png";
    public override Node2D CreateCustomSprite()
    {
        return PreloadManager.Cache.GetScene($"{rootPathScene}stone_cube_orb.tscn").Instantiate<Node2D>();
    }

    //被动
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature target)
    {
        
    }

    //激发
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        return [Owner.Creature];
    }
}
