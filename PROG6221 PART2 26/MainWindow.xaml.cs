using System.Media;
using System.Windows;
using System.Windows.Documents;

namespace CyberSecurityChatBot
{
    public partial class MainWindow : Window
    {
        ChatBot bot = new ChatBot();
        UserProfile user = new UserProfile();

        public MainWindow()
        {
            InitializeComponent();
            StartBot();
        }

        void StartBot()
        {
            PlayVoiceGreeting();
            AppendText("Bot: Hello! What is your name?\n");
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string input = txtInput.Text.Trim();
            txtInput.Clear();

            if (string.IsNullOrWhiteSpace(input))
            {
                AppendText("Bot: I didn’t understand that. Can you rephrase?\n");
                return;
            }

            AppendText("You: " + input + "\n");

            // FIRST INPUT = NAME
            if (string.IsNullOrEmpty(user.Name))
            {
                user.Name = input;
                AppendText($"Bot: Welcome {user.Name}! Ask me about cybersecurity.\n");
                return;
            }

            string response = bot.GetResponse(input, user);
            AppendText("Bot: " + response + "\n");
        }

        void AppendText(string text)
        {
            rtbChat.Document.Blocks.Add(new Paragraph(new Run(text)));
            rtbChat.ScrollToEnd();
        }

        void PlayVoiceGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("greeting.wav");
                player.Play();
            }
            catch
            {
                AppendText("Bot: (Voice greeting unavailable)\n");
            }
        }
    }
}