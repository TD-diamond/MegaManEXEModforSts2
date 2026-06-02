
# 卡牌描述与打出逻辑描述相关

## 选择敌人
在玩家的CombateState中可以获取敌人列表
以下是震荡波中获取全体敌人的方式
```csharp
    foreach (Creature enemy in base.CombatState.HittableEnemies)
    {
        ...
    }
```

## 获得能量
可以通过玩家控制台获得能量
```csharp
    await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
```

## 获取卡牌信息

### 获取卡牌能量
虽然我们直接通过卡牌的EnergyCost属性获取到的是一个被封装好的类。
我们要获取具体的数值则需要获取内部的Canonical属性
```csharp
    int amount = card.EnergyCost.Canonical + 1;
    //或者判断卡牌耗能数值
    card.EnergyCost.Canonical == 2
```

### 获取卡牌变量
通过CardAbstract实现了一个能够从外部获取卡牌变量的接口，
可以利用这个来检查卡牌是否拥有某个变量
```csharp
    card.GetDynamicVar("Block") != null
```

## 抽牌堆相关
大部分与抽牌堆相关的操作都可以通过CardPileCmd进行操作
```csharp
    //向抽牌堆添加牌
    await CardPileCmd.Add(item, PileType.Draw);
    //洗牌
    await CardPileCmd.ShuffleIfNecessary(choiceContext,Owner);
    //抽牌
    await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
```

## 根据玩家状态调整卡牌描述文本
如果想要根据玩家的条件筛选卡牌描述，则需要在卡牌内部创建一个可计算的变量
```csharp
protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("Distance").WithMultiplier((CardModel card, Creature _) => (card != null && card.IsMutable && card.Owner != null) ? card.Owner.Creature.GetPowerAmount<Distance>() : 0)
    ];
```
WithMultiplier传入multiplier函数，这里我们的条件是检查玩家身上的Distance能力数值。
这是对应的json文件
当Distance变量的数值大于1时，和Distance大于2时都会添加不同的文本描述
```json
"MEGAMANBATTLENETWORK-CIRCLE_GUN.description":"{DistanceCount:cond:>=1?对全体敌人|}造成{Damage:diff()}点伤害。{DistanceCount:cond:>=2?\n额外攻击选中的目标一次。|}
```


## 选择卡牌进行操作

选择卡牌是指我们选择卡牌，添加效果，或者将其放回牌组等操作

以下是变换剑的打出逻辑
```csharp

//获取选取的卡牌列表
//GetPile可以获取玩家的牌堆信息，这里cards可以获取内部的卡牌。
List<CardModel> cardsIn = PileType.Draw.GetPile(Owner).Cards
                                    .Where((CardModel card) => 
                                    {
                                        return card.Tags.Contains((CardTag)CustomCardTag.Sword)
                                                && !(card is VariantSword);
                                    }
                                    ).ToList();
        //之后从卡牌选择处选取卡牌
		CardModel cardModel = (
            await CardSelectCmd.FromSimpleGrid(
                context:choiceContext,
                cardsIn:cardsIn,
                //卡牌参数需要构造CardSelectionPrefs类
                //这里的SelectionScrennPrompt需要在cards.json中填写
                prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1),
                //prefs的构造函数在下面提供
                player: Owner)
                ).FirstOrDefault();
		if (cardModel != null)
		{
            CardModel card = cardModel.CreateClone();
            card.AddKeyword(CardKeyword.Exhaust);
            await CardCmd.AutoPlay(choiceContext,card,null);
		}
```
以下是CardSelectorPrefs的构造函数
传入时只需要指定最小选择数和最大选择数就行了
```csharp
public CardSelectorPrefs(LocString prompt, int selectCount)
    :this(prompt, selectCount, selectCount)
    {
        ...
    }

    public CardSelectorPrefs(LocString prompt, int minCount, int maxCount)
    {
        ...
    }
```

而这是对应的json文件的部分
```json
{
    ...
	"MEGAMANBATTLENETWORK-VARIANT_SWORD.title":"变换剑",
	"MEGAMANBATTLENETWORK-VARIANT_SWORD.description":"选择你抽牌堆中一张除自身以外，性质为'剑'的卡牌。\n打出其[gold]消耗[/gold]的复制品。",
	"MEGAMANBATTLENETWORK-VARIANT_SWORD.selectionScreenPrompt":"选择你要打出的卡牌",
    ...
}
```

需要注意的是，如果在战斗中生成选定的卡牌的话，用AddGeneratedCardToCombat来添加。
同时添加的一定要是克隆，而不是原卡牌。
如下
```csharp
CardModel card = cardToCopy.CreateClone();
        card.AddKeyword(CardKeyword.Exhaust);
        card.AddKeyword(CardKeyword.Ethereal);
        return CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, addedByPlayer: true);
```


## 为卡牌添加属性

卡牌的属性是卡牌打出去后对敌人施加的属性，可以被克制此属性的属性引爆，从而多造成一次伤害。
克制关系为
火->水->草->电->火
剑->浮游->瞄准->破坏->剑

为了让卡牌附带有这些属性，我们需要在卡牌的OnPlay函数中实施它
为了让卡牌添加属性，我们使用ToAttackWithStyle函数来施加属性，通过这个函数可以在之后角色攻击时给攻击到的敌人附加属性。
(原因是随机攻击和全体攻击很难被再去通过PowerCmd去添加，这里就使用了这种方式)
**但注意！这个函数只能用于单属性添加，双属性添加的函数还没做**
```csharp
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ToAttackWithStyle(choiceContext,cardPlay,
                    PowerTypes.Fire,async ()=>
                    {
                        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                        .WithHitCount(DynamicVars.Repeat.IntValue)
                        .FromCard(this)
                        .TargetingRandomOpponents(CombatState)
                        .Execute(choiceContext);
                    });
    }
```
因为还没做添加双属性的函数，如果想要添加双属性，就只能通过PowerCmd去添加了。

同时，为了让玩家知道这张牌是什么属性的，需要在ExtraHoverTip内添加HoverTip。
下方是噪音风暴的HoverTip。噪音风暴为浮游系
```csharp
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<FloatStylePower>(),
    ];
```
