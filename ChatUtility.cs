﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Speech.Synthesis;
using Microsoft.Speech.AudioFormat;


namespace TwitchChatSpeech
{
    static class ChatUtility
    {
        public static string DetectCommand(string chat)
        {
            string cmd = "add";
            return cmd;
        }

        public static Word TryExtractAddCommand(string chat)
        {
            Word word = new Word(@"([8]{4,})", "ぱちぱち");
            return word;
        }

        public static string Replace(string chat)
        {
            //Word[] words = new Word[]
            //{
            //    new Word{ 
            //        Pattern = @"([8]{4,})", 
            //        Replace = "ぱちぱち" 
            //    },
            //    new Word{ 
            //        Pattern = @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?", 
            //        Replace = "url" 
            //    }
            //};

            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline;

            foreach (var word in Words.Read())
            {
                Match match = Regex.Match(chat, word.Pattern, options);

                //Console.WriteLine(match.Success.ToString());
                if (match.Success)
                {
                    // replace
                    chat = chat.Replace(match.Value, word.Replace);
                    //Console.WriteLine(chat);
                }
            }

            return chat;
        }

        public static void Addreplace(string pattern, string replace)
        {
            Words.Add(pattern, replace);
        }

        // write part in new function
        


        public static void Urlfinder(string chat)
        {
            Regex re = new Regex(@"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Match m = re.Match(chat);
            for (int gIdx = 0; gIdx < m.Groups.Count; gIdx++)
            {
                Console.WriteLine("[{0}] = {1}", re.GetGroupNames()[gIdx], m.Groups[gIdx].Value);
            }
        }

        public static void RegexPatternTest(string text)
        {
            string replacement = "$1";
            string patternJapanese = $@"一-龯ぁ-んァ-ン";

            string pattern = $@"^([A-Za-z]+)[\s{patternJapanese}].*";
            //string pattern = $@"^([{patternJapanese}]+).*";

            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline;

            Match match = Regex.Match(text, pattern, options);
            if (match.Success)
            {
                Console.WriteLine("◆◆◆◆◆◆◆");
                Console.WriteLine(match.Result(replacement));
            }
        }

        public static void CheckInstalledVoices()
        {
            // Initialize a new instance of the SpeechSynthesizer.
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {

                // Output information about all of the installed voices. 
                Console.WriteLine("Installed voices -");
                foreach (InstalledVoice voice in synth.GetInstalledVoices())
                {
                    VoiceInfo info = voice.VoiceInfo;
                    string AudioFormats = "";
                    foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                    {
                        AudioFormats += String.Format("{0}\n",
                        fmt.EncodingFormat.ToString());
                    }

                    Console.WriteLine(" Name:          " + info.Name);
                    Console.WriteLine(" Culture:       " + info.Culture);
                    Console.WriteLine(" Age:           " + info.Age);
                    Console.WriteLine(" Gender:        " + info.Gender);
                    Console.WriteLine(" Description:   " + info.Description);
                    Console.WriteLine(" ID:            " + info.Id);
                    Console.WriteLine(" Enabled:       " + voice.Enabled);
                    if (info.SupportedAudioFormats.Count != 0)
                    {
                        Console.WriteLine(" Audio formats: " + AudioFormats);
                    }
                    else
                    {
                        Console.WriteLine(" No supported audio formats found");
                    }

                    string AdditionalInfo = "";
                    foreach (string key in info.AdditionalInfo.Keys)
                    {
                        AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
                    }

                    Console.WriteLine(" Additional Info - " + AdditionalInfo);
                    Console.WriteLine();
                }
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}