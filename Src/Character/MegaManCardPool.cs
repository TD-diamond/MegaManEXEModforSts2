using BaseLib.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaManBattleNetwork.Src.Character;

public class MegaManCardPool : CustomCardPoolModel
{
    public override string Title => MegaMan.CharacterId; 
    public override string BigEnergyIconPath => $"res://MegaManBattleNetwork/Images/Character/Energy/megaman_energy_icon.png";        //大能量标志
    public override string TextEnergyIconPath => $"res://MegaManBattleNetwork/Images/Character/Energy/megaman_energy_icon.png";      //文本能量标志
    public override Color DeckEntryCardColor => new Color("00BFFF");    //角色卡牌主色调，深天蓝
    public override Color ShaderColor => new("00BFFF");     //卡牌默认外框

    public override bool IsColorless => false;              //不是无色卡池
}
