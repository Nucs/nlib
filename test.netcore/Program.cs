using System;
using System.Diagnostics;
using nucs.JsonSettings;

namespace test.netcore
{
    class Program
    {

        public class ooo : EncryptedJsonSettings {
            public override string FileName { get; set; } = "lol.jsn";
            public string Yoooo { get; set; }
            public ooo() { }
            public ooo(string password) : base(password) { }
        }
        static void Main(string[] args) 
        {
            var sw = new Stopwatch();
            sw.Start();
            var vb = EncryptedJsonSettings.Load<ooo>();
            vb.Yoooo = "xoxo";
            vb.Save();
            //var vbb = JsonSettings.Load(new SettingsBag(), "lolnub");
            /*            vb.Autosave = true;

                        vb["kek"] = "lol";
                        vb.Save();*/

            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
