using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Rooms;
using MegaManBattleNetwork.Src.Character;
using MegaManBattleNetwork.Src.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManBattleNetwork.Src.Relics;

[Pool(typeof(MegaManRelicPool))]
public class Pet():RelicAbstracts(RelicRarity.Starter)
{

    public override string PackedIconPath => $"{relicImgPathRoot}/pet.png";
    protected override string PackedIconOutlinePath => $"{relicImgPathRoot}/pet.png";
    protected override string BigIconPath => $"{relicImgPathRoot}/pet_big.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>    
    [
        HoverTipFactory.FromPower<PanelsTaken>(),
    ];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            await PowerCmd.Apply<PanelsTaken>(Owner.Creature,9,Owner.Creature,null);
        }
    }
}