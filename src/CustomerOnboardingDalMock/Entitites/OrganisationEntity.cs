using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class OrganisationEntity
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public byte[]? Logo { get; set; } = default!;
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DbConnectionString { get; set; } = string.Empty;
        public int SubscriptionKind { get; set; }
        public int NumberOfEmployees { get; set; }
        public DateTime? FirstPayday { get; set; }
        public int? Currency { get; set; }
        public int? PayFrequency { get; set; }
        public bool AutoRunPayroll { get; set; }
        public decimal RegularHours { get; set; }
        public bool AllowNegativePay { get; set; }
        public byte[] LastChanged { get; set; } = default!;
        public string TPIN { get; set; } = string.Empty;
        public string NAPSAAccountNumber { get; set; } = string.Empty;
        public string NHIMAAccountNumber { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;
        public string BankId { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public string BranchCode { get; set; } = string.Empty;
        public int? NumberOfWorkingDaysPerPayPeriod { get; set; }
        public DateTime? FirstPayDay { get; set; }
    }
       
}
