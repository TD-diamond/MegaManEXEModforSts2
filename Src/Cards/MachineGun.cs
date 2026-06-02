using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Character;
using MegaManBattleNetwork.Src.Powers;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class MachineGun():DistanceChangeEffectCard(1,CardType.Attack,CardRarity.Common,TargetType.RandomEnemy,true)
{
    public string cardID = "MachineGun";
    public override string PortraitPath => $"{cardImgPathRoot}/machine_gun.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    base.CanonicalVars.Concat(
    [
        new DamageVar(1,ValueProp.Move),
        new RepeatVar(3)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    base.ExtraHoverTips.Concat(    
    [
        HoverTipFactory.FromPower<AimStylePower>()
    ]);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int distanceAmont = Owner.Creature.GetPowerAmount<Distance>();
        int repeatTimes = DynamicVars.Repeat.IntValue;
        
        if(distanceAmont >= 1)
        {
            repeatTimes += 3;
            if(distanceAmont >= 2)
            {
                repeatTimes += 3;
            }
        }
        await ToAttackWithStyle(choiceContext,cardPlay,
                    PowerTypes.Aim,async ()=>
                    {
                        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                        .WithHitCount(repeatTimes)
                        .FromCard(this)
                        .TargetingRandomOpponents(CombatState)
                        .WithAttackerAnim("machine_gun",0.0f)
                        .Execute(choiceContext);
                    });
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
    }
}
