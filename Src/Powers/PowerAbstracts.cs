using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaManBattleNetwork.Src.Powers;

public abstract class PowerAbstracts: CustomPowerModel
{
     // 类型，Buff或Debuff
    public override PowerType Type  {get{return pwType;}}

    // 叠加类型，Counter表示可叠加，Single表示不可叠加
    public override PowerStackType StackType {get{return pwStkType;}}

    // 自定义图标路径。1:1即可。原版游戏大图256x256，小图64x64。

    public override string CustomPackedIconPath => $"{customIconRoot}/null_power.png";
    public override string CustomBigIconPath => $"{customIconRoot}/null_power.png";
    public PowerAbstracts(PowerType pwType,PowerStackType pwStkType)
    {
        this.pwType = pwType;
        this.pwStkType = pwStkType;
    }
    private PowerType pwType;
    private PowerStackType pwStkType;
    protected string customIconRoot = "res://MegaManBattleNetwork/Images/Powers";
}