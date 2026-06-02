using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaManBattleNetwork.Src.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManBattleNetwork.Src.Relics;

abstract public class RelicAbstracts:CustomRelicModel
{
    // 稀有度
    
    public RelicAbstracts(RelicRarity rarity)
    {
        this.rarity = rarity;
    }
    private readonly RelicRarity rarity;
    public override RelicRarity Rarity => rarity;

    // 遗物的数值。替换本地化中的{Cards}。
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected const string imgPathRoot = $"res://MegaManBattleNetwork/Images";
    protected const string relicImgPathRoot = $"{imgPathRoot}/Relics";
    // 小图标（原版85x85）
    public override string PackedIconPath => $"{relicImgPathRoot}/pet.png";
    // 轮廓图标（原版85x85）
    protected override string PackedIconOutlinePath => $"{relicImgPathRoot}/pet.png";
    // 大图标（原版256x256）
    protected override string BigIconPath => $"{relicImgPathRoot}/pet_big.png";

    // 初始遗物的升级可以写这里
    // public override RelicModel? GetUpgradeReplacement() => ModelDb.Relic<Circlet>().ToMutable();
}

