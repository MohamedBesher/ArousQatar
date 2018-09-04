namespace Saned.ArousQatar.Data.Core.Models
{
    public class EmailSetting : IEntityBase
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public string SubjectAr { get; set; }
        public string SubjectEn { get; set; }
        public string MessageBodyAr { get; set; }
        public string MessageBodyEn { get; set; }
        public string EmailSettingType { get; set; }

    }
}
