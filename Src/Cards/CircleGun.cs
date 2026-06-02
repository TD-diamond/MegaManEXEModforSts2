using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;
using MegaManBattleNetwork.Src.Character;
using MegaManBattleNetwork.Src.Powers;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class CircleGun():DistanceChangeEffectCard(1,CardType.Attack,CardRarity.Common,TargetType.AnyEnemy,true)
{
    //基础攻击8
    public string cardID = "circle_gun";

    //卡图，如果没有就注释掉
    //cardId
    //卡图为止为$"{cardImgPathRoot}/.png"
    public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";    //卡图  

    //卡牌内登记的变量，用于被json文件读取。      
    //base内自带DistanceCount计算变量
    //不过目前没找到使用时该如何调用DynamicVars内部的这个计算变量。
    //所以计算的时候可能还是得直接获取能力的数值
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    base.CanonicalVars.Concat(
    [
        new DamageVar(10,ValueProp.Move),
    ]);

    //卡牌的关键字
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    //卡牌的额外提示
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    base.ExtraHoverTips.Concat(
    [
        HoverTipFactory.FromPower<AimStylePower>()
    ]);


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        //卡牌打出的效果逻辑
        if(Owner.HasPower<Distance>())
        {
            int amount = Owner.Creature.GetPowerAmount<Distance>();

            if(amount >= 1)
            {
                await ToAttackWithStyle(choiceContext,cardPlay,
                PowerTypes.Aim,async ()=>
                {
                    await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this)
                    .TargetingAllOpponents(CombatState)
                    .Execute(choiceContext);
                }); 
                if(amount >= 2)
                {
                    await ToAttackWithStyle(choiceContext,cardPlay,
                    PowerTypes.Aim,async ()=>
                    {
                        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                        .FromCard(this)
                        .Targeting(cardPlay.Target)
                        .Execute(choiceContext);
                    });   
                }
            }
            else
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);   
            }        
        }
        else
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);    
        }
	}

    protected override void OnUpgrade()
    {
        //升级后改变哪些东西
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}