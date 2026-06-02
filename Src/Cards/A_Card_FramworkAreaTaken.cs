// using System;
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
// using MegaCrit.Sts2.Core.Models;
// using MegaCrit.Sts2.Core.Models.Powers;
// using MegaCrit.Sts2.Core.ValueProps;
// using MegaManBattleNetwork.Src.Cards;
// using MegaManBattleNetwork.Src.Character;
// using MegaManBattleNetwork.Src.Powers;

// namespace MegaManBattleNetwork.Src.Cards;

// [Pool(typeof(MegaManCardPool))]
// public class AreaTakenFramework()
//     :AreaTakenCards(1,3,CardType.Skill,CardRarity.Uncommon,TargetType.Self,true)
// {
//     public string cardID = "ColorPoint";

//     public override string PortraitPath => $"{cardImgPathRoot}/color_point.png";    //卡图    

//     public override IEnumerable<CardKeyword> CanonicalKeywords =>   [CardKeyword.Exhaust];

//     //卡牌的额外提示
//     //一定要继承基类的ExtraHoverTips
//     //需要添加其他的HoverTip就使用其内部的.contact()函数，连接另一个Enumerable集合
//     protected override IEnumerable<IHoverTip> ExtraHoverTips =>
//     base.ExtraHoverTips.contact(
//     []
//     );

//     protected override IEnumerable<DynamicVar> CanonicalVars =>
//     base.ExtraHoverTips.contact(
//     []
//     );
//     protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//     {
//          //打出的效果逻辑
//          await base.OnPlay(choiceContext,cardPlay);
//     }
//         
//     protected override void OnUpgrade()
//     {
//        
//     }
// }