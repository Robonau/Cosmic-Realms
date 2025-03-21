﻿using System;
using common.resources;
using wServer.networking.packets.outgoing;
using wServer.realm;

namespace wServer.logic.behaviors
{
    class Grenade : Behavior
    {
        //State storage: cooldown timer

        double range;
        float radius;
        double? fixedAngle;
        int damage;
        Cooldown coolDown;
        ConditionEffectIndex effect;
        int effectDuration;
        uint color;

        public Grenade(double radius, int damage, double range = 5,
            double? fixedAngle = null, Cooldown coolDown = new Cooldown(), ConditionEffectIndex effect = 0, int effectDuration = 0, uint color = 0xffff0000)
        {
            this.radius = (float)radius;
            this.damage = damage;
            this.range = range;
            this.fixedAngle = fixedAngle * Math.PI / 180;
            this.coolDown = coolDown.Normalize();
            this.effect = effect;
            this.effectDuration = effectDuration;
            this.color = color;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = 0;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            int cool = (int)state;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffects.Stunned))
                    return;

                var player = host.AttackTarget ?? host.GetNearestEntity(range, true);
                if (player != null || fixedAngle != null)
                {
                    Position target;
                    if (fixedAngle != null)
                        target = new Position()
                        {
                            X = (float)(range * Math.Cos(fixedAngle.Value)) + host.X,
                            Y = (float)(range * Math.Sin(fixedAngle.Value)) + host.Y,
                        };
                    else
                        target = new Position()
                        {
                            X = player.X,
                            Y = player.Y,
                        };
                    host.Owner.BroadcastPacketNearby(new ShowEffect()
                    {
                        EffectType = EffectType.Throw,
                        Color = new ARGB(color),
                        TargetObjectId = host.Id,
                        Pos1 = target
                    }, host, null, PacketPriority.Low);
                    host.Owner.Timers.Add(new WorldTimer(1500, (world, t) =>
                    {
                        world.BroadcastPacketNearby(new networking.packets.outgoing.Aoe()
                        {
                            Pos = target,
                            Radius = radius,
                            Damage = (ushort)damage,
                            Duration = effectDuration,
                            Effect = effect,
                            OrigType = host.ObjectType,
                            ObjectName = host.ObjectDesc.DisplayId ?? host.ObjectDesc.ObjectId
                            
                        }, host, null, PacketPriority.Low);
                    }));
                }
                cool = coolDown.Next(Random);
            }
            else
                cool -= time.ElaspedMsDelta;

            state = cool;
        }
    }
}
