using KLibrary.Labs.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace UnitTest.Media
{
    [TestClass]
    public class WavesPlayerTest
    {
        const string Empty = "Empty";
        const string Success = "Success";
        const string Fail = "Fail";

        [TestMethod]
        public void Play()
        {
            var sounds = new[] { Empty, Success, Fail }
                .ToDictionary(n => n, n => string.Format(@"Sounds\{0}.wav", n));

            using (var player = new WavesPlayer(sounds))
            {
                player.Play(Empty);
                Thread.Sleep(200);
                player.Play(Success);
                Thread.Sleep(1200);
                player.Play(Fail);
                Thread.Sleep(1200);
            }
        }
    }
}
