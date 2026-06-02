using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaManBattleNetwork.Src.Character;
using MegaManBattleNetwork.Src.Powers;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class KnightSword() : DistanceChangeEffectCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
{
    public string cardID = "knight_sword";
    public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        base.CanonicalVars.Concat(
        [
            new DamageVar(12, ValueProp.Move),
        ]);

    public override IEnumerable<CardTag> Tags => [(CardTag)CustomCardTag.Sword];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        base.ExtraHoverTips.Concat(
        [
            HoverTipFactory.FromPower<SwordStylePower>()
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ToAttackWithStyle(choiceContext, cardPlay, PowerTypes.Sword, async () =>
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);

            int distanceAmount = Owner.Creature.GetPowerAmount<Distance>();
            if (distanceAmount < 1)
            {
                return;
            }

            decimal splashDamage = DynamicVars.Damage.BaseValue * 0.75m;
            List<Creature> otherEnemies = CombatState.GetTeammatesOf(cardPlay.Target)
                .Where(enemy => enemy.IsHittable && enemy != cardPlay.Target)
                .ToList();

            if (otherEnemies.Count == 0)
            {
                return;
            }

            if (distanceAmount >= 2)
            {
                foreach (Creature enemy in otherEnemies)
                {
                    await DamageCmd.Attack(splashDamage)
                        .FromCard(this)
                        .Targeting(enemy)
                        .Execute(choiceContext);
                }
                return;
            }

            Random random = new Random();
            int targetCount = Math.Min(2, otherEnemies.Count);
            for (int i = 0; i < targetCount; i++)
            {
                int pickIndex = random.Next(otherEnemies.Count);
                Creature pickedEnemy = otherEnemies[pickIndex];
                otherEnemies.RemoveAt(pickIndex);

                await DamageCmd.Attack(splashDamage)
                    .FromCard(this)
                    .Targeting(pickedEnemy)
                    .Execute(choiceContext);
            }
        });
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }
}
