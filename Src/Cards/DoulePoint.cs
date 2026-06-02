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
public class DoublePoint()
    :AreaTakenCards(1,3,CardType.Skill,CardRarity.Rare,TargetType.Self,true)
{
    public string cardID = "DoublePoint";

    public override string PortraitPath => $"{cardImgPathRoot}/double_point.png";    //卡图    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>    
    base.ExtraHoverTips.Concat(
    [
        HoverTipFactory.FromPower<VigorPower>(),
    ]
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    base.CanonicalVars.Concat(
    [
        new PowerVar<VigorPower>(8)
    ]);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<PanelsTaken>(Owner.Creature,DynamicVars.Power<PanelsTaken>().IntValue,Owner.Creature,this);
        await PowerCmd.Apply<VigorPower>(Owner.Creature,DynamicVars.Power<VigorPower>().IntValue,Owner.Creature,this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Power<VigorPower>().UpgradeValueBy(2);
    }
}