using BaseLib;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;
using MegaManBattleNetwork.Src.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class DoubleBeasts():CardAbstract(2,CardType.Attack,CardRarity.Uncommon,TargetType.RandomEnemy,true)
{
    public string cardID = "DoubleBeasts";
    public override string PortraitPath => $"{cardImgPathRoot}/double_beasts.png";                                             //卡图
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(2,ValueProp.Move),
        new RepeatVar(7)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .TargetingRandomOpponents(CombatState)
                .WithHitCount(DynamicVars.Repeat.IntValue)
                .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
    }
}