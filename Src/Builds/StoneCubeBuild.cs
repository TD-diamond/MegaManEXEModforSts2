
using MegaManBattleNetwork.Src.Builds;

public class StoneCubeBuild:AbstractBuild
{
    // 根据进阶提高最小血量，进阶8及以上为120，否则为100
    public override int MinInitialHp => 10;

    // 根据进阶提高最大血量，进阶8及以上为140，否则为120
    public override int MaxInitialHp => 10;
}