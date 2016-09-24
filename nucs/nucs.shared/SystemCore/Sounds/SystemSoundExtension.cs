using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;

namespace nucs.SystemCore.Sounds
{
    public static class SystemSoundExtension {
        /// <summary>
        ///     Plays a specific sound n times with delay of 500ms between them
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="times"></param>
        public static void Play(this SystemSound ss, int times) {
            for (int i = 0; i < times; i++) {
                ss.Play();
                Thread.Sleep(500);
            }
        }
    }
}
