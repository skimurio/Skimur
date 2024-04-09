namespace Skimur.Web.ViewModels.Messages
{
    public class ComposeMessageViewModel
    {
        public bool IsModerator { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}
