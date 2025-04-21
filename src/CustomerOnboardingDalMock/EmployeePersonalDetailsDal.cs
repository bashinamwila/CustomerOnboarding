using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.DalMock.Entitites;


namespace CustomerOnboarding.DalMock
{
    public class EmployeePersonalDetailsDal :
        IEmployeePersonalDetailsDal
    {
        public bool Exists(string employeeId)
        {
            var result = MockDb.EmployeePersonalDetails.
                        Any(r => r.EmployeeId == employeeId);
            return result;

        }

        public EmployeePersonalDetailsDto Fetch(string tenantId, string employeeId)
        {
            var result = MockDb.EmployeePersonalDetails.Where(r => r.TenantId == tenantId &&
                r.EmployeeId == employeeId)
                .Select(r => new EmployeePersonalDetailsDto
                {
                    Gender = r.Gender,
                    MaritalStatus = r.MaritalStatus,
                    EmailAddress = r.EmailAddress,
                    AddressLine1 = r.AddressLine1,
                    AddressLine2 = r.AddressLine2,
                    SSN = r.SSN,
                    DateOfBirth = r.DateOfBirth,
                    Nationality = r.Nationality,
                    IdType = r.IdType,
                    Id = r.Id,
                    TPIN = r.TPIN,
                    NHIMAAccountNumber = r.NHIMAAccountNumber,
                    PhoneNo = r.PhoneNo,
                    IsDifferentlyAbled = r.IsDifferentlyAbled,
                    NextOfKin = r.NextOfKin,
                    RelationWithNextOfKin = r.RelationWithNextOfKin,
                    NextOfKinPhoneNo = r.NextOfKinPhoneNo,
                    LastChanged = r.LastChanged,
                }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("EmployeePersonalDetails");
            return result;
        }

        public void Insert(EmployeePersonalDetailsDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new EmployeePersonalDetailsEntity
            {
                TenantId = dto.TenantId,
                EmployeeId = dto.EmployeeId,
                Gender = dto.Gender,
                MaritalStatus = dto.MaritalStatus,
                EmailAddress = dto.EmailAddress,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                SSN = dto.SSN,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                IdType = dto.IdType,
                Id = dto.Id,
                TPIN = dto.TPIN,
                NHIMAAccountNumber = dto.NHIMAAccountNumber,
                PhoneNo = dto.PhoneNo,
                IsDifferentlyAbled = dto.IsDifferentlyAbled,
                NextOfKin = dto.NextOfKin,
                RelationWithNextOfKin = dto.RelationWithNextOfKin,
                NextOfKinPhoneNo = dto.NextOfKinPhoneNo,
                LastChanged = dto.LastChanged,
            };
            MockDb.EmployeePersonalDetails.Add(newItem);
        }

        public void Update(EmployeePersonalDetailsDto dto)
        {
            var result = MockDb.EmployeePersonalDetails.Where(r => r.TenantId == dto.TenantId &&
               r.EmployeeId == dto.EmployeeId)
               .Select(r => r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("EmployeePersonalDetails");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("EmployeePersonalDetails");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.Gender = dto.Gender;
            result.MaritalStatus = dto.MaritalStatus;
            result.EmailAddress = dto.EmailAddress;
            result.AddressLine1 = dto.AddressLine1;
            result.AddressLine2 = dto.AddressLine2;
            result.SSN = dto.SSN;
            result.DateOfBirth = dto.DateOfBirth;
            result.Nationality = dto.Nationality;
            result.IdType = dto.IdType;
            result.Id = dto.Id;
            result.TPIN = dto.TPIN;
            result.NHIMAAccountNumber = dto.NHIMAAccountNumber;
            result.PhoneNo = dto.PhoneNo;
            result.IsDifferentlyAbled = dto.IsDifferentlyAbled;
            result.NextOfKin = dto.NextOfKin;
            result.RelationWithNextOfKin = dto.RelationWithNextOfKin;
            result.NextOfKinPhoneNo = dto.NextOfKinPhoneNo;
            result.LastChanged = dto.LastChanged;
        }
    }
}
