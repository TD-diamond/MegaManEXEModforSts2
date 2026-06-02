using BaseLib.Abstracts;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManBattleNetwork.Src.Character;

public class MegaManPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => MegaMan.Color;

    public override string BigEnergyIconPath => $"res://MegaManBattleNetwork/Images/Character/Energy/megaman_energy_icon.png";        //大能量标志
    public override string TextEnergyIconPath => $"res://MegaManBattleNetwork/Images/Character/Energy/megaman_energy_icon.png";      //文本能量标志
}