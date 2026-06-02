using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Character;
using MegaManBattleNetwork.Src.Powers;

namespace MegaManBattleNetwork.Src.Cards;


[Pool(typeof(MegaManCardPool))]
public class Muramasa():CardAbstract(2,CardType.Attack,CardRarity.Rare,TargetType.AnyEnemy,true)
{
    public string cardID = "Muramasa";

    public override string PortraitPath => $"{cardImgPathRoot}/muramasa.png";    //卡图    

    protected override HashSet<CardTag> CanonicalTags => [(CardTag)CustomCardTag.Sword];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>    
    [
        HoverTipFactory.FromPower<SwordStylePower>()
    ];

    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new CalculationBaseVar(0),
		new ExtraDamageVar(1),
        new CalculatedDamageVar(ValueProp.Move)
            .WithMultiplier((CardModel card, Creature _)
                => (card.Owner.Creature.MaxHp - card.Owner.Creature.CurrentHp))          
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        await PowerCmd.Apply<SwordStylePower>(cardPlay.Target,DynamicVars.CalculatedDamage.IntValue,Owner.Creature,this);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
    
}
