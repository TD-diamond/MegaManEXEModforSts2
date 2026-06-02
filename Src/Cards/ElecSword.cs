using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Character;
using MegaManBattleNetwork.Src.Powers;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class ElecSword():CardAbstract(1,CardType.Attack,CardRarity.Common,TargetType.AnyEnemy,true)
{
    public string cardID = "ElecSword";
    public override string PortraitPath => $"{cardImgPathRoot}/elec_sword.png";    //卡图   
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(7, ValueProp.Move),
    ];
    public override IEnumerable<CardTag> Tags => [(CardTag)CustomCardTag.Sword];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>    
    [
        HoverTipFactory.FromPower<ElecStylePower>(),
        HoverTipFactory.FromPower<SwordStylePower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await DamageCmd.Attack(
            DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        await PowerCmd.Apply<ElecStylePower>(cardPlay.Target,DynamicVars.Damage.IntValue,Owner.Creature,this);
        await PowerCmd.Apply<SwordStylePower>(cardPlay.Target,DynamicVars.Damage.IntValue,Owner.Creature,this);
	}

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}