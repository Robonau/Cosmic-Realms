﻿using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;


namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ HermitGod = () => Behav()
            .Init("Hermit God",
                new State(
                   new ScaleHP2(85, 2, 15),
                    new DropPortalOnDeath("Ocean Trench Portal", 1),
                    new OrderOnDeath(20, "Hermit God Tentacle Spawner", "Die", 1),
                    new OrderOnDeath(20, "Hermit God Drop", "Die", 1),
                    new Spawn("Hermit God Drop"),
                    new State("Spawn Tentacle",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new SetAltTexture(2),
                        new Order(20, "Hermit God Tentacle Spawner", "Tentacle"),
                        new EntityExistsTransition("Hermit God Tentacle", 20, "Sleep"),
                        new TimedTransition(10000, "Sleep")
                        ),
                    new State("Sleep",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Order(20, "Hermit God Tentacle Spawner", "Minions"),
                        new TimedTransition(1000, "Waiting")
                        ),
                    new State("Waiting",
                        new SetAltTexture(3),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new EntityNotExistsTransition("Hermit God Tentacle", 20, "Wake")
                        ),
                    new State("Wake",
                        new SetAltTexture(2),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TossObject("Hermit Minion", 10, angle: 0, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 45, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 90, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 135, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 180, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 225, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 270, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 315, throwEffect: true),
                        new TimedTransition(100, "Spawn Whirlpool")
                        ),
                    new State("Spawn Whirlpool",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Order(20, "Hermit God Tentacle Spawner", "Whirlpool"),
                        new EntityExistsTransition("Whirlpool", 20, "Attack1")
                        ),
                    new State("Attack1",
                        new SetAltTexture(0),
                        new Prioritize(
                            new Wander(0.3),
                            new StayCloseToSpawn(0.5, 5)
                            ),
                        new Shoot(20, count: 3, shootAngle: 5, coolDown: 300),
                        new TimedTransition(6000, "Attack2")
                        ),
                    new State("Attack2",
                        new Prioritize(
                            new Wander(0.3),
                            new StayCloseToSpawn(0.5, 5)
                            ),
                        new Order(20, "Whirlpool", "Die"),
                        new Shoot(20, count: 1, defaultAngle: 0, fixedAngle: 0, rotateAngle: 45, projectileIndex: 1, coolDown: 1000),
                        new Shoot(20, count: 1, defaultAngle: 0, fixedAngle: 180, rotateAngle: 45, projectileIndex: 1, coolDown: 1000),
                        new TimedTransition(6000, "Spawn Tentacle")
                    )
                ),
                new Threshold(0.00001,
                    new ItemLoot("Potion of Vitality", 0.3, 3),
                    new ItemLoot("Potion of Dexterity", 0.3, 3),
                    new TierLoot(8, ItemType.Weapon, 0.25),
                    new TierLoot(9, ItemType.Weapon, 0.125),
                    new TierLoot(8, ItemType.Armor, 0.25),
                    new TierLoot(9, ItemType.Armor, 0.25),
                    new TierLoot(3, ItemType.Ring, 0.25),
                    new TierLoot(4, ItemType.Ability, 0.125),
                    new TierLoot(4, ItemType.Ring, 0.125),
                    new TierLoot(10, ItemType.Weapon, 0.0625),
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(10, ItemType.Armor, 0.125),
                    new TierLoot(11, ItemType.Armor, 0.125),
                    new TierLoot(12, ItemType.Armor, 0.0625),
                    new TierLoot(5, ItemType.Ability, 0.0625),
                    new TierLoot(5, ItemType.Ring, 0.0625),
                    new ItemLoot("Potion of Dexterity", 0.4, 1),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Fragment of the Earth", 0.01),
                    new ItemLoot("Coral Bleached Shuriken", 0.006, damagebased: true),
                    new ItemLoot("Helm of the Juggernaut", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Hermit Shell Armor", 0.006, damagebased: true),
                    new TierLoot(2, ItemType.Potion, numRequired: 1)
    )
            )
            .Init("Hermit Minion",
                new State(
                    new Prioritize(
                        new Follow(0.6, 4, 1),
                        new Orbit(0.6, 10, 15, "Hermit God", speedVariance: .2, radiusVariance: 1.5),
                        new Wander(0.6)
                        ),
                    new Shoot(6, count: 3, shootAngle: 10, coolDown: 1000),
                    new Shoot(6, count: 2, shootAngle: 20, projectileIndex: 1, coolDown: 2600, predictive: 0.8)
                    )
            )
            .Init("Whirlpool",
                new State(
                    new State("Attack",
                        new EntityNotExistsTransition("Hermit God", 100, "Die"),
                        new Prioritize(
                            new Orbit(0.3, 6, 10, "Hermit God")
                            ),
                        new Shoot(0, 1, fixedAngle: 0, rotateAngle: 30, coolDown: 400)
                        ),
                    new State("Die",
                        new Shoot(0, 8, fixedAngle: 360/8),
                        new Suicide()
                        )
                    )
            )
            .Init("Hermit God Tentacle",
                new State(
                    new Prioritize(
                        new Follow(0.6, 4, 1),
                        new Orbit(0.6, 6, 15, "Hermit God", speedVariance: .2, radiusVariance: .5)
                        ),
                    new Shoot(3, count: 8, shootAngle: 360/8, coolDown: 500)
                    )
            )
            .Init("Hermit God Tentacle Spawner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Waiting Order"),
                    new State("Tentacle",
                        new Reproduce("Hermit God Tentacle", 3, 1, coolDown: 2000),
                        new EntityExistsTransition("Hermit God Tentacle", 1, "Waiting Order")
                        ),
                    new State("Whirlpool",
                        new Reproduce("Whirlpool", 3, 1, coolDown: 2000),
                        new EntityExistsTransition("Whirlpool", 1, "Waiting Order")
                        ),
                    new State("Minions",
                        new Reproduce("Hermit Minion", 40, 20, coolDown: 1000),
                        new TimedTransition(2000, "Waiting Order")
                        ),
                    new State("Die",
                        new Suicide()
                        )
                    )
            )
            .Init("Hermit God Drop",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Waiting"),
                    new State("Die",
                        new GroundTransform("Shallow Water", 5),
                        new Decay(2000)
                        )
                    )
            )
            ;
    }
}