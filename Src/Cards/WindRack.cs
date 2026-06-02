
using System.Buffers;
using System.Collections.Generic;
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
public class WindRack():CardAbstract(1,CardType.Attack,CardRarity.Uncommon,TargetType.AnyEnemy,true)
{
    public string cardID = "WindRack";
    public override string PortraitPath => $"{cardImgPathRoot}/wind_rack.png";    //卡图 

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>    
    [
        HoverTipFactory.FromPower<FloatStylePower>()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(11,ValueProp.Move),
        new PowerVar<WindRackPower>(-2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        await PowerCmd.Apply<WindRackPower>(Owner.Creature,-2,Owner.Creature,null);
        await PowerCmd.Apply<FloatStylePower>(cardPlay.Target,DynamicVars.Damage.IntValue,Owner.Creature,this);
        await CreatureCmd.LoseBlock(cardPlay.Target,999);
	}

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }
}