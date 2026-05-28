using System;
using System.Collections.Generic;

namespace CyberSecurityChatBot
{
    public class ChatBot
    {
        private Dictionary<string, List<string>> responses;
        private Random rand = new Random();

        public string CurrentTopic { get; private set; }

        public ChatBot()
        {
            responses = new Dictionary<string, List<string>>()
            {
                {
                    "password", new List<string>()
                    {
                        "Use strong passwords with uppercase, lowercase, numbers, and symbols.",
                        "Avoid using the same password on multiple sites.",
                        "Use a password manager for safety."
                    }
                },
                {
                    "phishing", new List<string>()
                    {
                        "Phishing scams trick users into giving sensitive info.",
                        "Always verify email senders before clicking links.",
                        "Watch for urgent or suspicious messages."
                    }
                },
                {
                    "privacy", new List<string>()
                    {
                        "Check your privacy settings regularly.",
                        "Do not overshare personal info online.",
                        "Enable two-factor authentication."
                    }
                }
            };
        }

        public string GetResponse(string input, UserProfile user)
        {
            input = input.ToLower();

            // SENTIMENT
            if (input.Contains("worried") || input.Contains("scared"))
            {
                return "I understand your concern. Let me help you stay safe.";
            }

            // MEMORY
            if (input.Contains("i like"))
            {
                user.FavoriteTopic = input.Replace("i like", "").Trim();
                return $"Great! I'll remember that you like {user.FavoriteTopic}.";
            }

            // FOLLOW-UP
            if (input.Contains("tell me more") || input.Contains("explain"))
            {
                if (!string.IsNullOrEmpty(CurrentTopic))
                {
                    return GetRandomResponse(CurrentTopic);
                }
            }

            // KEYWORDS
            foreach (var key in responses.Keys)
            {
                if (input.Contains(key))
                {
                    CurrentTopic = key;

                    string response = GetRandomResponse(key);

                    if (user.FavoriteTopic == key)
                    {
                        response += $"\nSince you like {key}, you should explore this more!";
                    }

                    return response;
                }
            }

            // DEFAULT
            return "I didn’t understand that. Can you rephrase?";
        }

        private string GetRandomResponse(string topic)
        {
            var list = responses[topic];
            return list[rand.Next(list.Count)];
        }
    }
}