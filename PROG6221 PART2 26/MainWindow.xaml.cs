using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Documents;

namespace CyberSecurityChatBot
{
    public partial class MainWindow : Window
    {
        string userName = "";
        string favoriteTopic = "";
        string currentTopic = "";

        // 🔁 Tracks which stage of the onboarding flow we are in
        enum BotStage
        {
            AskingName,
            AskingSecurityLevel,
            Chatting
        }

        BotStage currentStage = BotStage.AskingName;

        Random rand = new Random();

        Dictionary<string, List<string>> responses;

        public MainWindow()
        {
            InitializeComponent();
            InitializeBot();
            ShowLogo();
            ShowTextGreeting();
            StartBot();

            // ✅ Play voice greeting AFTER window is fully loaded
            this.Loaded += (s, e) => PlayVoiceGreeting();
        }

        // 🔥 ASCII LOGO
        void ShowLogo()
        {
            AppendText("===================================================\n");
            AppendText("           CYBERSECURITY AWARENESS BOT\n");
            AppendText("===================================================\n");
            AppendText("        .----.\n");
            AppendText("       / .--. \\\n");
            AppendText("      | |    | |\n");
            AppendText("      | |.-\"\"-.|\n");
            AppendText("     ///`.::::.`\\\n");
            AppendText("    ||| ::/  \\:: ;\n");
            AppendText("    ||; ::\\__/:: ;\n");
            AppendText("     \\\\\\ '::::' /\n");
            AppendText("      `=':-..-'`\n");
            AppendText("        CYBER SECURITY\n");
            AppendText("===================================================\n\n");
        }

        // 💬 TEXT GREETING shown after logo
        void ShowTextGreeting()
        {
            AppendText("👋 Welcome to the Cybersecurity Awareness Bot!\n");
            AppendText("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n");
            AppendText("🔐 I'm BOLT — your personal cybersecurity guide.\n");
            AppendText("   I'm here to help you stay safe in the digital world.\n\n");
            AppendText("💡 You can ask me about:\n");
            AppendText("   • Passwords       • Phishing attacks\n");
            AppendText("   • Safe browsing   • Online privacy\n");
            AppendText("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n");
        }

        // 🔊 VOICE GREETING
        void PlayVoiceGreeting()
        {
            try
            {
                string fullPath = Path.GetFullPath("greeting.wav");

                if (File.Exists(fullPath))
                {
                    mediaPlayer.Source = new Uri(fullPath, UriKind.Absolute);
                    mediaPlayer.Play();
                }
                else
                {
                    // Fallback: Windows built-in chime if no .wav file found
                    SystemSounds.Asterisk.Play();
                }
            }
            catch (Exception ex)
            {
                AppendText($"Bot: Audio error: {ex.Message}\n");
            }
        }

        void InitializeBot()
        {
            responses = new Dictionary<string, List<string>>()
            {
                {
                    "password", new List<string>()
                    {
                        "Use strong passwords with numbers, symbols and uppercase letters.",
                        "Never reuse passwords across accounts.",
                        "Use a password manager for safety."
                    }
                },
                {
                    "phishing", new List<string>()
                    {
                        "Phishing tricks users into giving personal info.",
                        "Always check email senders carefully.",
                        "Avoid clicking suspicious links."
                    }
                },
                {
                    "privacy", new List<string>()
                    {
                        "Review your privacy settings regularly.",
                        "Don't overshare personal information.",
                        "Enable two-factor authentication."
                    }
                }
            };
        }

        void StartBot()
        {
            AppendText("Bot: Hello! Before we begin, what is your name?\n");
        }

        // SEND BUTTON
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string input = txtInput.Text.Trim();
            txtInput.Clear();

            if (string.IsNullOrWhiteSpace(input))
            {
                AppendText("Bot: Please type something.\n");
                return;
            }

            AppendText("You: " + input + "\n");
            ProcessInput(input.ToLower());
        }

        void ProcessInput(string input)
        {
            // ── STAGE 1: Ask for name ──────────────────────────────────────
            if (currentStage == BotStage.AskingName)
            {
                userName = input;
                AppendText($"\nBot: Great to meet you, {userName}! 👋\n\n");
                AppendText("Bot: Before we dive in, I'd love to know your current\n");
                AppendText("     level of cybersecurity knowledge.\n\n");
                AppendText("     Please type one of the following:\n\n");
                AppendText("     1 - Beginner    (I'm new to cybersecurity)\n");
                AppendText("     2 - Intermediate (I know the basics)\n");
                AppendText("     3 - Advanced     (I'm fairly experienced)\n\n");
                currentStage = BotStage.AskingSecurityLevel;
                return;
            }

            // ── STAGE 2: Ask security level ───────────────────────────────
            if (currentStage == BotStage.AskingSecurityLevel)
            {
                if (input == "1" || input.Contains("beginner") || input.Contains("new"))
                {
                    AppendText($"\nBot: No worries, {userName}! Everyone starts somewhere. 🌱\n");
                    AppendText("Bot: I'll keep things simple and easy to understand.\n\n");
                    AppendText("Bot: Here's a quick tip to get you started:\n");
                    AppendText("     🔑 Always use a strong, unique password for each account.\n\n");
                }
                else if (input == "2" || input.Contains("intermediate") || input.Contains("basic"))
                {
                    AppendText($"\nBot: Nice! You've got a head start, {userName}. 💪\n");
                    AppendText("Bot: I'll help you sharpen what you already know.\n\n");
                    AppendText("Bot: Here's something to think about:\n");
                    AppendText("     🎣 Phishing attacks are getting smarter — always\n");
                    AppendText("        verify the sender before clicking any link.\n\n");
                }
                else if (input == "3" || input.Contains("advanced") || input.Contains("experienced"))
                {
                    AppendText($"\nBot: Impressive, {userName}! A cyber-savvy user. 🛡️\n");
                    AppendText("Bot: Let's keep your knowledge razor sharp.\n\n");
                    AppendText("Bot: Here's an advanced reminder:\n");
                    AppendText("     🔒 Regularly audit your privacy settings and\n");
                    AppendText("        enable two-factor authentication everywhere.\n\n");
                }
                else
                {
                    AppendText("Bot: Please type 1, 2, or 3 to select your level.\n");
                    return;
                }

                AppendText("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n");
                AppendText($"Bot: Alright {userName}, you're all set! 🚀\n");
                AppendText("Bot: Feel free to ask me anything about:\n");
                AppendText("     password  |  phishing  |  privacy\n");
                AppendText("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n");
                currentStage = BotStage.Chatting;
                return;
            }

            // ── STAGE 3: Normal chat ──────────────────────────────────────
            if (currentStage == BotStage.Chatting)
            {
                // SENTIMENT
                if (input.Contains("worried") || input.Contains("scared"))
                {
                    AppendText("Bot: I understand your concern. Stay safe online. 🛡️\n");
                }

                // MEMORY
                if (input.Contains("i like"))
                {
                    favoriteTopic = input.Replace("i like", "").Trim();
                    AppendText($"Bot: I'll remember you like {favoriteTopic}. 👍\n");
                    return;
                }

                // FOLLOW-UP
                if (input.Contains("tell me more") || input.Contains("explain"))
                {
                    if (!string.IsNullOrEmpty(currentTopic))
                    {
                        AppendText("Bot: Here's more about " + currentTopic + ":\n");
                        AppendText(GetRandomResponse(currentTopic) + "\n");
                        return;
                    }
                    else
                    {
                        AppendText("Bot: What topic would you like me to explain? Try: password, phishing, or privacy.\n");
                        return;
                    }
                }

                // KEYWORDS
                foreach (var key in responses.Keys)
                {
                    if (input.Contains(key))
                    {
                        currentTopic = key;
                        string reply = GetRandomResponse(key);

                        if (favoriteTopic == key)
                        {
                            reply += " Since you like this topic, explore it further!";
                        }

                        AppendText("Bot: " + reply + "\n");
                        return;
                    }
                }

                // DEFAULT
                AppendText("Bot: I didn't understand that. Try asking about: password, phishing, or privacy.\n");
            }
        }

        string GetRandomResponse(string topic)
        {
            var list = responses[topic];
            return list[rand.Next(list.Count)];
        }

        void AppendText(string text)
        {
            rtbChat.Document.Blocks.Add(new Paragraph(new Run(text)));
            rtbChat.ScrollToEnd();
        }
    }
}