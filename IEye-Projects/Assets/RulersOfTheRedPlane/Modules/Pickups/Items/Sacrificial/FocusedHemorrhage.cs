﻿using MSU;
using MSU.Config;
using RoR2;
using RoR2.Items;
using R2API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R2API.Networking;
using RoR2.ContentManagement;

namespace IEye.RRP.Items
{
    public class FocusedHemorrhage : RRPItem
    {
        public const string token = "RRP_ITEM_FHEMORRHAGE_DESC";
        //public override ItemDef ItemDef { get; } = RRPAssets.LoadAsset<ItemDef>("FocusedHemorrhage", RRPBundle.Items);

        public override RRPAssetRequest AssetRequest => RRPAssets.LoadAssetAsync<ItemAssetCollection>("acFocusHemorrhage", RRPBundle.Items);

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Chance on hit to apply Hemorrhage(default 10%).")]
        [FormatToken(token, 0)]
        public static float percentChance = 10f;

        /*[RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Should there be reduced damage on the first stack?(default true).")]
        public static bool damageReduceOnOneStack = true;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Percent of damage on the first stack(if enabled)(default 40%).")]
        public static float percentDamageReduceOnOneStack = 40f;
        */
        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Added to damage multiplier(default 35%).")]
        
        public static float percentDamageOver = 35f;

        [FormatToken(token, 1)]
        public static float percentDamage = 100f + percentDamageOver;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Duration of Hemorrhage(default 10s).")]
        [FormatToken(token, 2)]
        public static float duration = 10f;




        public sealed class Behavior : BaseItemBodyBehavior, IOnDamageDealtServerReceiver
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.FocusedHemorrhage;

            public void OnDamageDealtServer(DamageReport report)
            {
                if (report.damageInfo.procCoefficient == 0f)
                {
                    return;
                }
                TeamIndex teamIndex = report.attackerBody.teamComponent.teamIndex;
                if (Util.CheckRoll(percentChance * report.damageInfo.procCoefficient, report.attackerBody.master))
                {
                    HealthComponent victim = report.victim;
                    InflictDotInfo dotInfo;
                    
                    
                     dotInfo = new InflictDotInfo()
                     {
                         attackerObject = report.attacker,
                         victimObject = victim.gameObject,
                         dotIndex = DotController.DotIndex.SuperBleed,
                         duration = report.damageInfo.procCoefficient * duration,
                         damageMultiplier = 1f + (percentDamageOver / 100),
                     };
                    
                    for(int i = 0; i < ((stack - 1) / 2) + 1; i++)
                    {
                        DotController.InflictDot(ref dotInfo);
                    }
                    
                }
            }

            private void Start()
            {
                if (body.inventory.GetItemCount(RRPContent.Items.SacrificialHelper) == 0)
                {
                    body.inventory.GiveItem(RRPContent.Items.SacrificialHelper);
                }
            }
            
        }

        public override void Initialize()
        {

        }

        public override bool IsAvailable(ContentPack contentPack)
        {
            return true;
        }
    }
}
