#region

using System;
using System.IO;
using System.Threading;
using FlaUI.Core;
using FlaUI.UIA3;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace Chapter05.Q44.Tests
{
    public class AutomationTest
    {
        public AutomationTest(ITestOutputHelper output)
        {
            _output = output;
        }

        private readonly string targetApplicationPath = "Chapter05.Q44.exe";
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// アプリの起動をテストします。
        /// </summary>
        [Fact]
        public void LaunchTest()
        {
            var app = Application.Launch(targetApplicationPath);
            try
            {
                using (var automation = new UIA3Automation())
                {
                    app.GetMainWindow(automation);
                    Thread.Sleep(1000);
                }
            }
            finally
            {
                app.Close();
            }
        }

        [Fact]
        public void SaveScreenShot()
        {
            var app = Application.Launch(targetApplicationPath);
            try
            {
                using (var automation = new UIA3Automation())
                {
                    var window = app.GetMainWindow(automation);
                    var listBox = window.FindFirstDescendant(factory => factory.ByAutomationId("ListBoxSentence"))
                        ?.AsListBox();
                    Assert.NotNull(listBox);
                    listBox.Select(3);
                    var textBox = window.FindFirstDescendant(factory => factory.ByAutomationId("TextBoxSentence"))
                        ?.AsTextBox();
                    Assert.NotNull(textBox);
                    Assert.Equal(listBox.SelectedItem.Text, textBox.Text);
                    Capture.Element(window)
                        .ToFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.png"));
                    Thread.Sleep(1000);
                }
            }
            finally
            {
                app.Close();
            }
        }

        /// <summary>
        /// リストボックスの選択をテストします。
        /// </summary>
        [Fact]
        public void SelectListBoxTest()
        {
            var app = Application.Launch(targetApplicationPath);
            try
            {
                using (var automation = new UIA3Automation())
                {
                    var window = app.GetMainWindow(automation);
                    var automationElement =
                        window.FindFirstDescendant(factory => factory.ByAutomationId("ListBoxSentence"));
                    Assert.NotNull(automationElement);
                    var listBox = automationElement.AsListBox();
                    listBox.Select(3);
                    Thread.Sleep(1000);
                }
            }
            finally
            {
                app.Close();
            }
        }

        /// <summary>
        /// テキストボックスに値がセットされたことをテストします。
        /// </summary>
        [Fact]
        public void SetTextBoxTest()
        {
            var app = Application.Launch(targetApplicationPath);
            try
            {
                using (var automation = new UIA3Automation())
                {
                    var window = app.GetMainWindow(automation);
                    var listBox = window.FindFirstDescendant(factory => factory.ByAutomationId("ListBoxSentence"))
                        ?.AsListBox();
                    Assert.NotNull(listBox);
                    listBox.Select(3);
                    var textBox = window.FindFirstDescendant(factory => factory.ByAutomationId("TextBoxSentence"))
                        ?.AsTextBox();
                    Assert.NotNull(textBox);
                    Assert.Equal(listBox.SelectedItem.Text, textBox.Text);
                    _output.WriteLine(textBox.Text);
                    Thread.Sleep(1000);
                }
            }
            finally
            {
                app.Close();
            }
        }
    }
}