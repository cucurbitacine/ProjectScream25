using Game.Scripts.Combat;
using Game.Scripts.Control;
using StateMachines.Data;
using StateMachines.Presets;
using StateMachines.Utils;
using UnityEngine;

namespace Game.Scripts.StateMachine.Features
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Features/Attack Feature", fileName = "AttackFeature", order = 0)]
    public class AttackFeature : FeaturePreset
    {
        [Min(1)]
        public int DamageAmount = 1;
        
        [Space]
        public float AttackDelay = 0f;
        public float AttackDuration = 0f;

        [Space]
        [Min(0f)] public float RadiusAttack = 0.5f;
        [Min(0f)] public float DistanceAttack = 0.5f;
        
        public override IFeatureProcess CreateProcess()
        {
            return new AttackProcess();
        }
    }

    public sealed class AttackProcess : FeatureProcess<AttackFeature>
    {
        [InjectComponent] private AttackController attacker;
        [InjectComponent] private VisualBody visual;

        private bool needAttack;
        private ContactFilter2D overlapFilter;
        
        private readonly Collider2D[] targets = new Collider2D[4];
        
        public bool IsAttackCompleted { get; private set; }
        public bool IsAttacking
        {
            get => attacker.IsAttacking;
            private set => attacker.IsAttacking = value;
        }

        public float LastAttackTime
        {
            get => attacker.LastAttackTime;
            private set => attacker.LastAttackTime = value;
        }
        
        public override void Enter()
        {
            base.Enter();

            IsAttackCompleted = false;
            IsAttacking = true;
            
            needAttack = true;
        }

        public override void Exit()
        {
            base.Exit();

            IsAttackCompleted = false;
            IsAttacking = false;
        }

        public override void Execute(float deltaTime)
        {
            base.Execute(deltaTime);

            if (IsAttackCompleted) return;

            if (needAttack)
            {
                if (TimeInState >= Preset.AttackDelay)
                {
                    Attack();
                    
                    LastAttackTime = Time.time;
                    
                    needAttack = false;
                }
            }

            if (TimeInState >= Preset.AttackDuration)
            {
                IsAttackCompleted = true;
            }
        }
        
        private void Attack()
        {
            overlapFilter = new ContactFilter2D()
            {
                useLayerMask = true,
                layerMask = attacker.TargetLayer,
                useTriggers = true,
            };

            var attackPoint = visual.Container.TransformPoint(Vector2.up * Preset.DistanceAttack);
            
            var gizmosData = attacker.gizmosData;
            gizmosData.center = attackPoint;
            gizmosData.radius = Preset.RadiusAttack;
            attacker.gizmosData = gizmosData;
            
            var count = Physics2D.OverlapCircle(attackPoint, Preset.RadiusAttack, overlapFilter, targets);

            if (count <= 0) return;

            for (var i = 0; i < count; i++)
            {
                var target = targets[i];
                if (target.TryGetComponent<Hitbox>(out var hitbox) && !attacker.Contains(hitbox))
                {
                    if (attacker.Team > 0 && attacker.Team == hitbox.Team)
                    {
                        continue;
                    }
                    
                    hitbox.Damage(Preset.DamageAmount);
                }
            }
        }
    }
}