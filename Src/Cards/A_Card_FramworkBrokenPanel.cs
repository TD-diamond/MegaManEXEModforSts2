// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using BaseLib.Utils;
// using Godot;
// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Localization.DynamicVars;
// using MegaCrit.Sts2.Core.Nodes.Rooms;
// using MegaCrit.Sts2.Core.Nodes.Vfx;
// using MegaCrit.Sts2.Core.Saves;
// using MegaCrit.Sts2.Core.Settings;
// using MegaCrit.Sts2.Core.ValueProps;
// using MegaManBattleNetwork.Src.Character;
// using MegaCrit.Sts2.Core.Helpers;
// using MegaCrit.Sts2.Core.Nodes;
// using MegaCrit.Sts2.Core.Commands.Builders;
// using MegaCrit.Sts2.Core.Entities.Creatures;
// using MegaCrit.Sts2.Core.Models;
// using MegaCrit.Sts2.Core.CardSelection;
// using MegaCrit.Sts2.Core.Models.Powers;
// using MegaCrit.Sts2.Core.Models.Cards;
// using MegaCrit.Sts2.Core.HoverTips;
// using MegaManBattleNetwork.Src.Powers;

// namespace MegaManBattleNetwork.Src.Cards;

// [Pool(typeof(MegaManCardPool))]
// public class CardFramework(): BrokenPanelCards(1,2,CardType.Attack,CardRarity.Common,TargetType.AnyEnemy,true)
// {
//     public string cardID = "double_throw";

//     //卡图，如果没有就注释掉
//     //cardId
//     //卡图为止为$"{cardImgPathRoot}/.png"
//     public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";    //卡图  

//     //卡牌内登记的变量，用于被json文件读取。      
//     protected override IEnumerable<DynamicVar> CanonicalVars =>
//     base.CanonicalVars.Concat(
//     []
//     );

//     //卡牌的关键字
//     public override IEnumerable<CardKeyword> CanonicalKeywords => [];

//     //卡牌的额外提示
//     //一定要继承基类的ExtraHoverTips
//     //需要添加其他的HoverTip就使用其内部的.contact()函数，连接另一个Enumerable集合
//     protected override IEnumerable<IHoverTip> ExtraHoverTips =>
//     base.ExtraHoverTips.contact(
//     []
//     );


//     protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//     {
//          //打出的效果逻辑
//          await base.OnPlay(choiceContext,cardPlay);
//     }

//     protected override void OnUpgrade()
//     {
//         //升级后改变哪些东西
//     }
// }