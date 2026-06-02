using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;
using MegaManBattleNetwork.Src.Character;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class MegaBuster() 
            : CardAbstract(0,CardType.Attack,CardRarity.Basic,TargetType.AnyEnemy,true)
{
    public string cardID = "MegaBuster";
    public override string PortraitPath => $"{cardImgPathRoot}/mega_buster.png";

    public override IEnumerable<CardTag> Tags => [(CardTag)CustomCardTag.Buster];

    public decimal RepeatTimes 
    { 
        get 
        { 
            return DynamicVars.Repeat.BaseValue; 
        } 
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(4, ValueProp.Move),
        new RepeatVar(1)
    ];
    public void SetRepeatValue(int v)
    {
        DynamicVars.Repeat.BaseValue = v;
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(
            DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithAttackerAnim("mega_buster",0.0f)
            .WithHitCount((int)RepeatTimes)
            .Execute(choiceContext);
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        if(player == Owner)
        {
            CardPile pile = Pile;
			if (pile == null || pile.Type != PileType.Hand)
			{
				await CardPileCmd.Add(this, PileType.Hand);
			}
        }
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1); 
    }
}