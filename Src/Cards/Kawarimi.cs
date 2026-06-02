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
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaManBattleNetwork.Src.Character;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class Kawarimi():CardAbstract(1,CardType.Power,CardRarity.Uncommon,TargetType.Self,true)
{
    public string cardID = "Kawarimi";
    public override string PortraitPath => $"{cardImgPathRoot}/kawarimi.png";    //卡图                           
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BufferPower>(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BufferPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature,"cast",0.0f);
        await PowerCmd.Apply<BufferPower>(Owner.Creature,DynamicVars["BufferPower"].BaseValue,Owner.Creature,this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Power<BufferPower>().UpgradeValueBy(1);
    }

}
