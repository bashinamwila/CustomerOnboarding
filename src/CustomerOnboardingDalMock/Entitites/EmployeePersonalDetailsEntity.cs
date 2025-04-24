using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class EmployeePersonalDetailsEntity
    {
        public string TenantId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public int? Gender { get; set; }
        public int? MaritalStatus { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public string SSN { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public int? Nationality { get; set; }
        public int? IdType { get; set; }
        public string Id { get; set; } = string.Empty;
        public string TPIN { get; set; } = string.Empty;
        public string NHIMAAccountNumber { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public bool IsDifferentlyAbled { get; set; }
        public string NextOfKin { get; set; } = string.Empty;
        public string RelationWithNextOfKin { get; set; } = string.Empty;
        public string NextOfKinPhoneNo { get; set; } = string.Empty;
        public byte[] LastChanged { get; set; } = default!;
    }
}
