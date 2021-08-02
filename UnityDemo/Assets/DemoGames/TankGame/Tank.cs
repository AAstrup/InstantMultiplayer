using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.DemoGames.TankGame
{
    public class Tank: MonoBehaviour
    {
        public int ShotsLeft;
        public bool Invincible;

        private void Start()
        {
            ShotsLeft = 3;
        }
    }
}
