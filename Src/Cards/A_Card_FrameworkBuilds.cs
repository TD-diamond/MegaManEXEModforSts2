
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using BaseLib.Extensions;
// using BaseLib.Utils;
// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.HoverTips;
// using MegaCrit.Sts2.Core.Localization.DynamicVars;
// using MegaCrit.Sts2.Core.Models.Powers;
// using MegaCrit.Sts2.Core.ValueProps;
// using MegaManBattleNetwork.Src.Character;
// using MegaManBattleNetwork.Src.Cmd;
// using MegaManBattleNetwork.Src.Orbs;
// using MegaManBattleNetwork.Src.Powers;

// namespace MegaManBattleNetwork.Src.Cards;

// [Pool(typeof(MegaManCardPool))]
// public class FrameWorkBuild():CardAbstract(1,CardType.Power,CardRarity.Common,TargetType.Self,true)
// {
//     public string cardID = "";
//     public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";  

//     protected override IEnumerable<DynamicVar> CanonicalVars =>
//     [];

//     protected override IEnumerable<IHoverTip> ExtraHoverTips =>    
//     [
//         //替换这里的AbstractOrb为任意继承AbstractOrb的Orb
//         HoverTipFactory.FromOrb<AbstractOrb>(),
//     ];

//     protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//     {
//         //这里也一样
//         await BuildCmd.Summon<T>(choiceContext);
//     }
//     protected override void OnUpgrade()
//     {
//         //升级后的逻辑随便写
//     }
// }