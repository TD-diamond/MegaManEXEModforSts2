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
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Cards;
using MegaManBattleNetwork.Src.Character;
using MegaManBattleNetwork.Src.Powers;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class PanelThrow():BrokenPanelCards(1,1,CardType.Attack,CardRarity.Basic,TargetType.AnyEnemy,true)
{
    public string cardID = "PanelThrow";
    public override string PortraitPath => $"{cardImgPathRoot}/panel_throw.png";  

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    base.CanonicalVars.Concat(
    [
        new DamageVar(6, ValueProp.Move),
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    base.ExtraHoverTips;

    protected override HashSet<CardTag> CanonicalTags =>
    [
        (CardTag)CustomCardTag.PanelThrowing
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await AttackWithAnim(choiceContext,
                            ()=>
                            {
                                return DamageCmd.Attack(DynamicVars.Damage.BaseValue) 
                                    .FromCard(this)
                                    .Targeting(cardPlay.Target);
                            });
        await base.OnPlay(choiceContext,cardPlay);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }

}