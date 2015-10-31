using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;

namespace nucs.SystemCore.Sounds
{
    public static class SystemSoundExtension {
        public static void Play(this SystemSound ss, int times) {
            for (int i = 0; i < times; i++) {
                ss.Play();
                Thread.Sleep(500);
            }
        }
    }
}
