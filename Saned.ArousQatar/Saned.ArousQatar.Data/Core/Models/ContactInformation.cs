namespace Saned.ArousQatar.Data.Core.Models
{
    public class ContactInformation : IEntityBase
    {

        public int Id { get; set; }
        public string Contact { get; set; }
        public int ContactTypeId { get; set; }
        public string IconName { get; set; }
        public virtual ContactType ContactType { get; set; }


        public void Modify(string contact, string iconName, int contactTypeId)
        {
            Contact = contact;
            IconName = iconName;
            ContactTypeId = contactTypeId;
        }
    }
}
