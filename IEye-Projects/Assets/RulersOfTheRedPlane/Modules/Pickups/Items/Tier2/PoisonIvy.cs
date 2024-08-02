﻿using System.Collections;
using UnityEngine;
using RoR2;
using RoR2.Items;
using Moonstorm;
using System.Collections.Generic;
using R2API.Networking;
using Moonstorm.Loaders;
using BepInEx.Logging;

namespace IEye.RRP.Items
{
    //[DisabledContent]
    public class PoisonIvy : ItemBase
    {

        private const string token = "RRP_ITEM_IVY_DESC";

        public static int buffsNeeded = 2;

        public static float distance = 15f;

        public static float baseDuration = 5f;

        public static float stackDuration = 2f;

        public static float waitTime = 10f;
        public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("PoisonIvy", RRPBundle.Items);
        

        public sealed class Behavior : BaseItemBodyBehavior
        {

            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.PoisonIvy;

            bool isRunning = false;
            private GameObject effect = RRPAssets.LoadAsset<GameObject>("PoisionIvyEffect", RRPBundle.Items);
            private void Update()
            {
                if(isRunning || body.activeBuffsListCount < buffsNeeded) return;
                int buffCount = 0;
                foreach(BuffIndex buffindex in body.activeBuffsList)
                {
                    if (!BuffCatalog.GetBuffDef(buffindex).isDebuff)
                    {
                        buffCount++;
                    }
                }
                
                if(buffCount >= buffsNeeded)
                {
                    applyPoision(PickNextTargets(body.healthComponent));
                    isRunning = true;
                    StartCoroutine(waitCoroutine());
                }
            }

            IEnumerator waitCoroutine()
            {
                yield return new WaitForSeconds(waitTime);
                isRunning = false;
            }
            public void applyPoision(List<HealthComponent> targets)
            {
                
                
                EffectManager.SpawnEffect(effect, 
                    new EffectData
                    {
                        origin = body.corePosition,
                        rotation = Quaternion.identity,
                        scale = distance,
                    },
                    true);
                if(targets == null)
                {
                    return;
                }
                foreach(HealthComponent target in targets)
                {
                    target.ApplyDot(body.gameObject, DotController.DotIndex.Poison, baseDuration + (stack-1 * stackDuration), 1);
                }
            }
            public List<HealthComponent> PickNextTargets(HealthComponent playerHealthComp)
            {

                if (this.search == null)
                {
                    this.search = new SphereSearch();
                }
                float range = baseRange;
                if (playerHealthComp && playerHealthComp.body)
                {
                    range += playerHealthComp.body.radius;
                }

                TeamMask mask = TeamMask.AllExcept(TeamIndex.Player);
                this.search.mask = LayerIndex.entityPrecise.mask;
                this.search.radius = range;
                this.search.origin = playerHealthComp.body.corePosition;
                this.search.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
                this.search.RefreshCandidates();
                this.search.FilterCandidatesByHurtBoxTeam(mask);
                this.search.FilterCandidatesByDistinctHurtBoxEntities();

                HurtBox[] hurtBoxes = this.search.GetHurtBoxes();
                healthComponents = new List<HealthComponent>();
                foreach (HurtBox hurtBox in hurtBoxes)
                {
                    if (hurtBox.healthComponent != playerHealthComp)
                    {
                        healthComponents.Add(hurtBox.healthComponent);
                    }
                }

                if (healthComponents.Count == 0)
                {
                    //DefNotSS2Log.Message("Search is null");
                    return null;
                }

                return healthComponents;



            }
            private SphereSearch search;
            private List<HealthComponent> healthComponents;
            public float baseRange = distance;
        }
    }
}


