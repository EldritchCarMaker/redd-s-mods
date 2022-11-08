using MoreCyclopsUpgrades.API.Upgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CyclopsCloakingMod_SN
{
    public class CloakUpgradeHandler : UpgradeHandler
    {
        public CloakUpgradeHandler(TechType techType, SubRoot cyclops) : base(techType, cyclops)
        {
            var cloak = cyclops.gameObject.EnsureComponent<Cloaking>();
            cloak.handler = this;
        }
    }
}
