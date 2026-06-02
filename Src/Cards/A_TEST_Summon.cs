
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using BaseLib.Extensions;
// using BaseLib.Hooks;
// using BaseLib.Utils;
// using Godot;
// using MegaCrit.Sts2.Core.Combat;
// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.Entities.Creatures;
// using MegaCrit.Sts2.Core.Entities.Players;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Hooks;
// using MegaCrit.Sts2.Core.HoverTips;
// using MegaCrit.Sts2.Core.Localization.DynamicVars;
// using MegaCrit.Sts2.Core.Models;
// using MegaCrit.Sts2.Core.Models.Monsters;
// using MegaCrit.Sts2.Core.Models.Powers;
// using MegaCrit.Sts2.Core.Nodes.Combat;
// using MegaCrit.Sts2.Core.Nodes.Rooms;
// using MegaCrit.Sts2.Core.TestSupport;
// using MegaCrit.Sts2.Core.ValueProps;
// using MegaManBattleNetwork.Src.Builds;
// using MegaManBattleNetwork.Src.Character;
// using MegaManBattleNetwork.Src.Cmd;
// using MegaManBattleNetwork.Src.Orbs;
// using MegaManBattleNetwork.Src.Powers;

// namespace MegaManBattleNetwork.Src.Cards;

// [Pool(typeof(MegaManCardPool))]
// public class Summon():CardAbstract(1,CardType.Power,CardRarity.Common,TargetType.Self,true)
// {
//     public string cardID = "";
//     // public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";  

//     protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//     {
//         //这里也一样

//         await BuildCmd.Summon<AbstractBuild>(choiceContext,Owner,this);
        
//         // await CreatureCmd.Add<AbstractBuild>(CombatState,null);


//     }
//     protected override void OnUpgrade()
//     {
//         //升级后的逻辑随便写
//     }
// }