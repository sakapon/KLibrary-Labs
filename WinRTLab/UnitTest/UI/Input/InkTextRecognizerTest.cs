using System;
using System.Threading;
using System.Windows;
using KLibrary.Labs.Reactive;
using KLibrary.Labs.UI.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.UI.Input
{
    [TestClass]
    public class InkTextRecognizerTest
    {
        [TestMethod]
        public void RecognizeAsync()
        {
            // UI スレッドでなければ動作しない？
            var recognizer = new InkTextRecognizer
            {
                Culture = InkTextRecognizer.Culture_ja_JP,
            };
            recognizer.BestResultArrived.Subscribe(Observer2.Create<string>(Console.WriteLine));

            recognizer.AddStroke(new[] { new Point(100, 100), new Point(100, 200) });
            recognizer.RecognizeAsync();

            Thread.Sleep(1000);
        }
    }
}
