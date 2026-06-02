using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;
using MegaManBattleNetwork.Src.Character;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class SpreadGun():CardAbstract(1,CardType.Attack,CardRarity.Common,TargetType.AllEnemies,true)
{
    public string cardID = "SpreadGun";

    public override IEnumerable<CardTag> Tags => [(CardTag)CustomCardTag.Gun];
    public override string PortraitPath => $"{cardImgPathRoot}/spread_gun.png";    //卡图                           
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8,ValueProp.Move)];

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(CombatState)
            .Execute(choiceContext);
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3); // 升级后伤害数+3
    }
}
