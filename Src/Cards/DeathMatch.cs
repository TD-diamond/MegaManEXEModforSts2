using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
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
public class DeathMatch():CardAbstract(1,CardType.Power,CardRarity.Uncommon,TargetType.Self,true)
{
    public string cardID = "death_match";

    public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";    //卡图  
   
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [];

    //卡牌的关键字
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    //卡牌的额外提示
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<BrokenPanels>(),
        HoverTipFactory.FromPower<PanelsTaken>(),
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(Owner.Creature,"cast",0.0f);
        //卡牌打出的效果逻辑
        int amount = Owner.Creature.GetPowerAmount<PanelsTaken>() - 1;
        await PowerCmd.Remove<BrokenPanels>(Owner.Creature);
        await PowerCmd.Apply<BrokenPanels>(Owner.Creature,amount,Owner.Creature,this);
	}

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
