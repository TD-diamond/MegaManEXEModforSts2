using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Character;
using MegaCrit.Sts2.Core.HoverTips;
using MegaManBattleNetwork.Src.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class StaticBlast(): CardAbstract(2,CardType.Attack,CardRarity.Uncommon,TargetType.AnyEnemy,true)
{
    public string cardID = "static_blast";

    //卡图，如果没有就注释掉
    //cardId
    //卡图为止为$"{cardImgPathRoot}/{cardID}.png"
    public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";    //卡图  

    //卡牌内登记的变量，用于被json文件读取。      
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(2,ValueProp.Move),
        new RepeatVar(4)
    ];

    //卡牌的额外提示
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<FloatStylePower>(),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>   [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        //卡牌打出的效果逻辑
        await CreatureCmd.TriggerAnim(Owner.Creature,"fan_active",0.0f);
        await ToAttackWithStyle(choiceContext,cardPlay,
            PowerTypes.Float,async ()=>
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                        .WithHitCount(DynamicVars.Repeat.IntValue)
                        .FromCard(this)
                        .Targeting(cardPlay.Target)
                        .WithAttackerAnim("fan_attack",0.0f)
                        .Execute(choiceContext);
            });
        await CreatureCmd.Stun(cardPlay.Target);
	}

    protected override void OnUpgrade()
    {
        //升级后改变哪些东西
        DynamicVars.Repeat.UpgradeValueBy(1);
    }
}