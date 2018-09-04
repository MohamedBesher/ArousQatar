namespace Saned.HandByHand.Data.Core.Dtos
{
    public class NotificationDto
    {
        public long ChatRequestId { get; set; }
        public string Message { get; set; }
        public int OverallCount { get; set; }
        public int NotifiedCount { get; set; }
        public bool Notified { get; set; }
        public int Id { get; set; }
        public int AdvertismentId { get; set; }
        public string Sender { get; set; }
        public string UserName { get; set; }
        public string PhotoUrl { get; set; }
        public string Name { get; set; }
        public string AdvertisementName { get; set; }

        

        public string Content
        {
            get
            {
                string message = " لقد ارسل لك  ";
                message += Name;
                message += "  رسالة جديدة ";
                message += " بخصوص ";
                message += AdvertisementName;
                return message;
            }
        }







    }
}