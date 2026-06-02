using BaseLib;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Character;
using MegaManBattleNetwork.Src.Orbs;
using MegaManBattleNetwork.Src.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManBattleNetwork.Src.Cards;

abstract public class CardAbstract(int cost, CardType type, CardRarity rarity, TargetType target,bool shouldShowInCardLibrary) :
    CustomCardModel(cost, type, rarity, target,shouldShowInCardLibrary)
{
    protected const string imgPathRoot = $"res://MegaManBattleNetwork/Images";
    protected const string cardImgPathRoot = $"{imgPathRoot}/Cards";
    public override string PortraitPath => $"{cardImgPathRoot}/chip_0_null.png";

    protected async Task ToAttackWithStyle(PlayerChoiceContext choiceContext, CardPlay cardPlay,
        PowerTypes powerType,Func<Task> func)
    {
        bool hadExistingMultipleAttackPower = Owner.Creature.HasPower<MultipleAttacksWithPower>();
            if (!hadExistingMultipleAttackPower)
            {
                await PowerCmd.Apply<MultipleAttacksWithPower>(Owner.Creature, 1, Owner.Creature, this);
                MultipleAttacksWithPower onHitPower = Owner.Creature.Powers.OfType<MultipleAttacksWithPower>().FirstOrDefault();
                onHitPower?.SetPower(powerType, DynamicVars.Damage.IntValue);
            }
            try
            {
                if(func != null)
                {
                    await func();
                }
            }
            finally
            {
                if (!hadExistingMultipleAttackPower && Owner.Creature.HasPower<MultipleAttacksWithPower>())
                {
                    await PowerCmd.Remove<MultipleAttacksWithPower>(Owner.Creature);
                }
            }
    }

    protected Task AttackWithAnim(PlayerChoiceContext choiceContext,Func<AttackCommand> func,float delay = 0.0f)
    {
        AttackCommand command = func();
        if(CanonicalTags.Contains((CardTag)CustomCardTag.Buster))
            command.WithAttackerAnim("mega_buster",delay,null);
        else if (CanonicalTags.Contains((CardTag)CustomCardTag.Sword))
            command.WithAttackerAnim("slash",delay,null);
        else if (CanonicalTags.Contains((CardTag)CustomCardTag.Throwing))
            command.WithAttackerAnim("throw",delay,null);
        else if (CanonicalTags.Contains((CardTag)CustomCardTag.PanelThrowing))
            command.WithAttackerAnim("panel_throw",delay,null);
        return command.Execute(choiceContext);
    }

    public DynamicVar GetVars(string varName)
    {
        if(!DynamicVars.ContainsKey(varName))
            return null;
        return DynamicVars[varName];
    }

    protected int GetNegativeAmount()
    {
        List<PowerModel> list = Owner.Creature.Powers.Where((PowerModel powerModel)
        =>
        {
            return powerModel is NegativePanels;
        }).ToList();
        int counts = 0;
        foreach(PowerModel pm in list)
        {
            counts += pm.Amount;
        }
        return counts;
    }

    protected int GetPanelAmount()
    {
        return Owner.Creature.GetPowerAmount<PanelsTaken>();
    }
    
}

abstract public class BrokenPanelCards:
    CardAbstract
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>    
    [
        HoverTipFactory.FromPower<BrokenPanels>(),
        HoverTipFactory.FromPower<BrokenPanelHintPower>(),
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<BrokenPanels>(brokenToPlay)
    ];
    public BrokenPanelCards(int cost,int BrokenToPlay,
         CardType type, CardRarity rarity, TargetType target,bool shouldShowInCardLibrary,
        int AreaDecreaseAmount = 0)
        : base(cost, type, rarity, target, shouldShowInCardLibrary)
    {
        brokenToPlay = BrokenToPlay;
        PanelAmount = AreaDecreaseAmount;
    }
    protected int brokenToPlay = 0;
    protected override bool IsPlayable =>
        (Owner.Creature.Powers
            .FirstOrDefault(p => p is PanelsTaken) as PanelsTaken)?.Amount
            - GetNegativeAmount()
            > brokenToPlay;

    protected override bool ShouldGlowGoldInternal => 
        (Owner.Creature.Powers
            .FirstOrDefault(p => p is PanelsTaken) as PanelsTaken)?.Amount 
            - GetNegativeAmount()
            > brokenToPlay;
    protected bool ShouldAreaDecreaseBePlay()
    {
        List<PowerModel> list = Owner.Creature.Powers.Where((PowerModel powerModel) =>{return powerModel is NegativePanels;}).ToList();
        int decreaseAmount = PanelAmount;
        foreach(PowerModel pm in list){
            decreaseAmount += pm.Amount;
        }
        int areaAmount = Owner.Creature.GetPower<PanelsTaken>().Amount;
        return decreaseAmount < areaAmount && decreaseAmount >= 0;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await PowerCmd.Apply<BrokenPanels>(Owner.Creature,brokenToPlay,Owner.Creature,this);
	}
    protected int PanelAmount;
}

abstract public class AreaTakenCards:
    CardAbstract
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>    
    [
        HoverTipFactory.FromPower<PanelsTaken>(),
        HoverTipFactory.FromPower<PanelTakenHintPower>(),
    ];
    public AreaTakenCards(int cost,int AreaToPlay,
    CardType type, CardRarity rarity, TargetType target,bool shouldShowInCardLibrary,
    int AreaDecreaseAmount = 0)
    : base(cost, type, rarity, target, shouldShowInCardLibrary)
    {
        areaToPlay = AreaToPlay;
        PanelAmount = AreaDecreaseAmount;
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<PanelsTaken>(-areaToPlay)
    ];
    
    protected int areaToPlay;
    protected override bool IsPlayable =>
        (Owner.Creature.Powers
            .FirstOrDefault(p => p is PanelsTaken) as PanelsTaken)?.Amount > areaToPlay
            && ShouldAreaDecreaseBePlay();

    protected override bool ShouldGlowGoldInternal => 
        (Owner.Creature.Powers
            .FirstOrDefault(p => p is PanelsTaken) as PanelsTaken)?.Amount > areaToPlay;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await PowerCmd.Apply<PanelsTaken>(Owner.Creature,DynamicVars.Power<PanelsTaken>().IntValue,Owner.Creature,this);
	}
    protected bool ShouldAreaDecreaseBePlay()
    {
        List<PowerModel> list = Owner.Creature.Powers.Where((PowerModel powerModel) =>{return powerModel is NegativePanels;}).ToList();
        int decreaseAmount = PanelAmount;
        foreach(PowerModel pm in list){
            decreaseAmount += pm.Amount;
        }
        int areaAmount = Owner.Creature.GetPower<PanelsTaken>().Amount;
        return decreaseAmount < areaAmount && decreaseAmount >= 0;
    }
    protected int PanelAmount;
}

public abstract class DistanceChangeEffectCard:CardAbstract
{
    public DistanceChangeEffectCard(int cost, CardType type,
        CardRarity rarity, TargetType target,bool shouldShowInCardLibrary)
        :base(cost,type,rarity,target,shouldShowInCardLibrary)
    {
        
    }

    //卡牌内登记的变量，用于被json文件读取。      
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        //为了能够被json文件读取的变量
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("DistanceCount").WithMultiplier((CardModel card, Creature _)
            => (card != null && card.IsMutable && card.Owner != null)
            ? card.Owner.Creature.GetPowerAmount<Distance>()
            :0)
        
    ];

    //卡牌的额外提示
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<Distance>(),
        HoverTipFactory.FromPower<DistanceChangeEffectsHint>()
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        //卡牌打出的效果逻辑
        //根据DistanceCount做出不同的动作
	}

    protected override void OnUpgrade()
    {
        //升级后改变哪些东西
    }
}
