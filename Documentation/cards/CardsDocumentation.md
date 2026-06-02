# 卡牌实现文档

## Area能力版实现
Area需要大于1，小于5，代表角色当前占地。
Area初始值为3。
当Area在2回合内没有改变时，则将Area重置为3。

能力版全部依照能力的数值，决定卡牌是否能够被打出。
需要重写卡牌的IsPlayable
```csharp
//当AreaPower大于1时，才可以打出此牌
protected override bool IsPlayable =>
        (Owner.Creature.Powers
            .FirstOrDefault(p => p is AreaPower) as AreaPower)?.Amount > 1;
```

同时为了让玩家知道此牌可以被打出，这里使用金光提示，之后应该会改
```csharp
protected override bool ShouldGlowGoldInternal => 
        (Owner.Creature.Powers
            .FirstOrDefault(p => p is AreaPower) as AreaPower)?.Amount > 1;
```

## 添加宠物

宠物是类似于佩尔士兵以及异鸟宝宝的那种，其实奥斯提也算，但是奥斯提比较特殊。

宠物本身通过MonsterModel实现，也就是宠物本身其实就是新怪物，先造新的怪物，再将其变成玩家的宠物。

想要在代码中获取到玩家宠物，就是用以下方式获取
```csharp
    //使用以下方式获取对应的宠物
    PaelsLegion paelsLegion = (PaelsLegion)base.Owner.PlayerCombatState.GetPet<PaelsLegion>().Monster;
    //使用以下方式获取宠物列表
    IReadOnlyList<Creature> list = Owner.PlayerCombatState.Pets.ToList();

    //使用时再转化为Creature
    //以下为触发动画的示例
    await CreatureCmd.TriggerAnim(paelsLegion.Creature, "WakeUpTrigger", 0.15f);

    //添加宠物
    await PlayerCmd.AddPet<MegaCrit.Sts2.Core.Models.Monsters.PaelsLegion>(base.Owner);
```


### 卡牌打出相关
是否能够打出卡牌只需要重写IsPlayable变量就可以了，它需要一个lambda函数作为参数
以下是"交锋"卡牌的IsPlayable变量的实现逻辑
```csharp
protected override bool IsPlayable => 
    CardPile.GetCards(base.Owner, PileType.Hand)
            .All(
                (CardModel c) =>
                {
                    c.Type == CardType.Attack
                }
            );
```

### 生物格挡
生物获得格挡时，使用CreatureCmd.GainBlock来获取格挡


### 获取对应Power数值
```csharp
    //以下是获取从卡牌中获取对应能量的方法
    if(!Owner.HasPower<AreaPower>())
        return;

    decimal amount = (Owner.Creature.Powers
            .FirstOrDefault(p => p is AreaPower) as AreaPower)?.Amount
            && ShouldAreaDecreaseBePlay();
```

### Panels
```csharp
    //为了将卡牌打出的地面限制，
    //会将玩家玩家的所有Panel类能力相加再加上此数值。
    //超过角色占地-1将无法打出
    //不为了不被限制就填0
    protected int Panels;
```



### 星星卡牌实现
重写卡牌内的 CanonicalStarCost。
并且，如果想要让角色使用Star，请在创建的角色内
```Csharp
public override bool ShouldAlwaysShowStarCounter => true;               //显示星星指示
```

# Cards.json的编写
所有卡牌以命名空间第一段大写，即第一个"."之前的段落，加上"-"，再加上卡牌类的名称
卡牌类的名称在次数编写时字母全大写，遇到驼峰时使用"_"断开，

比如，这个卡牌
```csharp
    namespace MegaManBattleNetwork.Cards;
    ...
    public class StepCross
    {
        ...
    }
```

对应的json文件应该这样编写"MEGAMANBATTLENETWORK-STEP_CROSS"

同时，卡牌会有title和description两个属性。
```json
"MEGAMANBATTLENETWORK-STEP_CROSS.title":"title内容",
"MEGAMANBATTLENETWORK-STEP_CROSS.description":"description内容"
```

