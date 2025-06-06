// PlayerStats.cs

using Buff;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    
    public class PlayerStats : CharacterStatsBase
    {
       
        
        [Header("当前的蛋属性")]
        public EggStats eggStats = new EggStats();
        

       
    }
}
