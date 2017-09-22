using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using nucs.JsonSettings;

/*using nucs.Automation;
using nucs.Automation.Controllers;
using nucs.Automation.Fluent;
using nucs.Automation.Mirror;
using nucs.Automation.Mirror.Helpers;
using nucs.Automation.Scripts;
using nucs.Filesystem;
using nucs.Threading.FastThreadPool;*/

namespace test {

 /*   public static class exts {
        public static FluentTemplate<TInv> ExecutingProcess<TInv>(this FluentTemplate<TInv> template, Action<TInv, FluentTemplate<TInv>, SmartProcess> act) where TInv : FluentInvoker {
            return template.Chain((invoker, fluentTemplate) => act(invoker, fluentTemplate, SmartProcess.This()));
        }

        public static Main() {
            var invoker = FluentInvoker.CreateDefault();
            var t = FluentInvoker.CreateTemplate(invoker);
            t.ExecutingProcess((fluentInvoker, template, smartproc) => {
                smartproc.Kill();
            });
        }
    }*/

    class Program {
        
        static unsafe void Main(string[] args) {
            var sw = new Stopwatch();
            sw.Start();
            var vb = JsonSettings.Load<SettingsBag>("lolnub");
            var vbb = JsonSettings.Load(new SettingsBag(),"lolnub");
            vb.Autosave = true;
            
            vb["kek"] = "lol";
            vb.Save();

/*            var invoker = FluentInvoker.CreateDefault();
            invoker.KeyboardController = new WindowSpecificKeyboardController(SmartProcess.Get("notepad"));
            invoker.Write("hello");
            invoker.Press(KeyCode.A);
            invoker.Press(KeyCode.A);
            invoker.Press(KeyCode.A);*/
            //var t = FluentInvoker.CreateTemplate(invoker);
            
                //.Invoke();

            //var f = new FastThreadPool(16);
            /*
                        var rects = new Rectangle(0, 0, 1920, 1080).Split(6, 6).ToArray();
                        foreach (var r in rects.Reverse()) {
                            ScreenDrawing.Draw(r);
                            Thread.Sleep(200);
                        }
            */


            /*

            var p = SmartProcess.Get("Mobizen");
            var t = p.Windows.ToArray();
            var wm = t.FirstOrDefault(f => f.Title == "watermark");
            wm.ChangeWindowState(WindowState.Minimized)
            ;
            //var a = DirectBitmap.FromFile("C:/lol/a.png");
            //var a = DirectBitmap.FromFile("C:/lol/a.png");
            var a = DirectBitmap.FromScreenshot(Screen.PrimaryScreen);
            var b = DirectBitmap.FromFile("C:/lol/b.png");
            
            var pos = a.FindSubimages(b, 0)[0];
            //var pos = imgs[0].RelativeToScreenshot();
            Mouse.AbsoluteMove(pos.X, pos.Y);
            for (int i = 0; i < 555555; i++) {
                Thread.Sleep(500);
                Console.WriteLine(Cursor.Position);
            }
            ;*/
            /*
                                                List<long> times = new List<long>(20);
                                                for (int i = 0; i < 20; i++) {
                                                    sw.Restart();
                                                    var t = ss.findImage(scr, 0);
                                                    sw.Stop();
                                                    Console.WriteLine(sw.ElapsedMilliseconds);
                                                    times.Add(sw.ElapsedMilliseconds);
                                                }
                                                Console.WriteLine("AVG: "+ times.Average());*/
            /*            var sw = new Stopwatch();
                                                var ss = DirectBitmap.FromScreenshot(Screen.PrimaryScreen);
                                                var scr = DirectBitmap.FromFile("C:/lol/subject.png");*/
            /*            var ss = DirectBitmap.FromFile("C:/lol/incr.png");
                                                var scr = DirectBitmap.FromFile("C:/lol/subincr.png");*/
            /*            for (int i = 0; i < ss.Height* ss.Width; i++) {
                                                    DirectBitmap.pixel* s = (DirectBitmap.pixel*)ss[i% ss.Width, i/ ss.Width];

                                                    Console.WriteLine(s->A);
                                                }*/
            /*            sw.Start();*/
            /*foreach (var rectangle in ss.findImage(scr, 0)) {
                //Console.WriteLine(rectangle);
            }*/
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        static Bitmap bm() {
            Bitmap screenshot = new Bitmap(SystemInformation.VirtualScreen.Width,
                SystemInformation.VirtualScreen.Height,
                PixelFormat.Format24bppRgb);

            // Create a graphics object from the bitmap.
            using (var gfxScreenshot = Graphics.FromImage(screenshot)) {
                // Take the screenshot from the upper left corner to the right bottom corner.
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                    Screen.PrimaryScreen.Bounds.Y,
                    0,
                    0,
                    Screen.PrimaryScreen.Bounds.Size,
                    CopyPixelOperation.SourceCopy);

                // Save the screenshot to the specified path that the user has chosen.
            }
            return screenshot;
        }
    }
}