using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.GameInfo.Objects;
using MegaManBattleNetwork.Src.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegaManBattleNetwork.Src.Powers;
using MegaCrit.Sts2.Core.Models.Cards;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class Uninstall():CardAbstract(3,CardType.Power,CardRarity.Rare,TargetType.Self,true)
{
    public string cardID = "Uninstall";
    public override string PortraitPath => $"{cardImgPathRoot}/uninstall.png";    //卡图    

    public override IEnumerable<CardKeyword> CanonicalKeywords =>   
    [
        CardKeyword.Ethereal
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<UninstallPower>(1),
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature,"cast",0.0f);
        await PowerCmd.Apply<UninstallPower>(Owner.Creature,DynamicVars.Power<UninstallPower>().BaseValue,Owner.Creature,this);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Ethereal);
    }
    
}
