

# 生成Orb/Build

## 充能球/造物的逻辑编写

需要注意一下，因为这里的造物(Build)是基于充能球(Orb)实现的，所以这里可能会用到充能球和造物两种称呼，但实际上指的是同一个东西。

所有的造物继承自AbstractOrb类
AbstractOrb的构造函数如下，需要提供一个被动值和一个激发值
```csharp
    public AstractOrb(decimal passive, decimal evoke = 0,bool DoFocusInfluenceValues = true)
```
造物这里一般是不会让它激发之后附带效果的，所以这里默认激发数值是0

触发球的被动效果就需要使用以下函数
```csharp
    //当球触发被动效果时，调用以下函数
    Passive(choiceContext, null);

    //Passive函数重写
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        Trigger();
    }

    // 触发激发，返回受影响的角色
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        PlayEvokeSfx();
        await CardPileCmd.Draw(playerChoiceContext, EvokeVal, Owner);
        return [Owner.Creature];
    }
```

## 充能球/造物的描述编写
相关描述全部储存在orbs.json文件内部，格式依旧是命名空间第一段大写 加上 "-" 加上 类名全大写，但是遇到驼峰用"_"断开
属性基本只有title，description和smartDescription几个属性
比如
```json
{
    "TEST-TEST_ORB.description": "充能球：回合开始时抽牌。",
    "TEST-TEST_ORB.smartDescription": "[gold]被动：[/gold]回合开始时，抽[blue]{Passive}[/blue]张牌。\n[gold]激发：[/gold]抽[blue]{Evoke}[/blue]张牌。",
    "TEST-TEST_ORB.title": "戈多球"
}
```

## 充能球/造物的代码框架
``` csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaManBattleNetwork.Src.Orbs;

public class StoneCubeOrb():AbstractOrb(15,5,false)
{
    // 暗色，使用球的主体色的暗色调
    public override Color DarkenedColor => new (0.1f,0.2f,0.5f);

    // 提示图标路径
    public override string CustomIconPath => $"{rootPathImg}stone_cube_orb.png";

    //充能球的场景
    public override Node2D CreateCustomSprite()
    {
        //关于场景的内容，在之后有说。
        return PreloadManager.Cache.GetScene($"{rootPathScene}stone_cube_orb.tscn").Instantiate<Node2D>();
    }

    //被动
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature target)
    {
        
    }

    //激发
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        return [Owner.Creature];
    }
}
```

## 创建充能球场景
充能球不仅仅需要图片，还需要场景。
不过照着以下的模板可以自定义场景内容。
[ext_resource]这栏里的path=""指定了节点所需图片的位置，更改这里就可以更改充能球的贴图。
第一个[node]这里可以更改其name为充能球的类名。
基本上只需要更改这两个地方，其他东西原封不动就行了。
```tscn
[gd_scene load_steps=2 format=3 uid="uid://megsnq8c4cxc"]

[ext_resource type="Texture2D" uid="uid://c0bqjvt4lhjm3" path="res://MegaManBattleNetwork/Images/Orbs/stone_cube_orb.png" id="1_xg8gl"]

[node name="StoneCubeOrb" type="Node2D"]

[node name="Icon" type="Sprite2D" parent="."]
scale = Vector2(0.4, 0.4)
texture = ExtResource("1_xg8gl")
```
**不过不要忘记,场景文件要放在Scenes目录下**

## 创建造物卡牌
已经做好了卡牌的框架，这里只需要指定卡牌生成的Orb然后填进去就可以了
```csharp

namespace MegaManBattleNetwork.Src.Cards;

[Pool(typeof(MegaManCardPool))]
public class FrameWorkBuild():CardAbstract(1,CardType.Power,CardRarity.Common,TargetType.Self,true)
{
    public string cardID = "";
    public override string PortraitPath => $"{cardImgPathRoot}/{cardID}.png";  

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>    
    [
        //替换这里的AbstractOrb为任意继承AbstractOrb的Orb
        HoverTipFactory.FromOrb<AbstractOrb>(),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        //这里也一样
        await OrbCmd.Channel<StoneCubeOrb>(choiceContext,Owner);
    }
    protected override void OnUpgrade()
    {
        //升级后的逻辑随便写
    }
}
```