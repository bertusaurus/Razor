﻿using System;
using System.IO;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using UOSteam;

namespace Assistant.Scripts
{
    public static class ScriptManager
    {
        public static bool Recording { get; set; }

        private static FastColoredTextBox _scriptEditor { get; set; }
        private class ScriptTimer : Timer
        {
            // Only run scripts once every 25ms to avoid spamming.
            public ScriptTimer() : base(TimeSpan.FromMilliseconds(25), TimeSpan.FromMilliseconds(25))
            {
            }

            protected override void OnTick()
            {                
                Interpreter.ExecuteScripts();
              
            }
        }

        private static ScriptTimer _timer { get; }

        static ScriptManager()
        {
            _timer = new ScriptTimer();
        }

        public static void SetControls(FastColoredTextBox scriptEditor)
        {
            _scriptEditor = scriptEditor;
        }

        public static void OnLogin()
        {
            Commands.Register();
            Aliases.Register();
            Expressions.Register();

            _timer.Start();
        }

        public static void OnLogout()
        {
            _timer.Stop();
        }

        public static void StartEngine()
        {
            _timer.Start();
        }

        public static void StopEngine()
        {
            _timer.Stop();
        }

        public static string[] GetScripts()
        {
            return Directory.GetFiles($"{Config.GetInstallDirectory()}\\Scripts", "*.razor");
        }

        public static bool AddToScript(string command)
        {
            if (Recording)
            {
                _scriptEditor?.AppendText(command + Environment.NewLine);

                return true;
            }

            return false;
        }

        public static void Error(string message, string scriptname = "")
        {
            World.Player?.SendMessage(MsgLevel.Error, $"Script '{scriptname}' error => {message}");
        }


    }
}