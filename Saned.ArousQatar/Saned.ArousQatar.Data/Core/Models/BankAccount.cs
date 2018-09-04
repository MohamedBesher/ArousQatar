namespace Saned.ArousQatar.Data.Core.Models
{
    public class BankAccount : IEntityBase
    {
        public int Id { get; set; }
        public string BankNumber { get; set; }
        public string BankName { get; set; }

        public void Modify(string bankNumber, string bankName)
        {
            BankName = bankName;
            BankNumber = bankNumber;
        }
    }
}
