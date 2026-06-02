using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Character;
using MegaManBattleNetwork.Src.Powers;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class ProtoMan():CardAbstract(2,CardType.Attack,CardRarity.Rare,TargetType.AllEnemies,true)
{
    public string cardID = "ProtoMan";
    public override string PortraitPath => $"{cardImgPathRoot}/delta_ray.png";    //卡图                           
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [new DamageVar(20,ValueProp.Move)];
    public override IEnumerable<CardTag> Tags => [(CardTag)CustomCardTag.Sword];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>    
    [
        HoverTipFactory.FromPower<SwordStylePower>()
    ];

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ToAttackWithStyle(choiceContext,cardPlay,
            PowerTypes.Sword,async ()=>
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .TargetingAllOpponents(CombatState)
                .Execute(choiceContext);
            });
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(8);
    }
    
}
