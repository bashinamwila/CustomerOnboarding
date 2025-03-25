using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.DalMock.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CustomerOnboarding.DalMock
{
    public class OrganisationDal :
        IOrganisationDal
    {
        public OrganisationDto Fetch(string id)
        {
            var result = (from r in MockDb.Organisations
                          where r.Id == id
            select new OrganisationDto
            {
                              Id = r.Id,
                              Name = r.Name,
                              Logo = r.Logo,
                              Country = r.Country,
                              AddressLine1 = r.AddressLine1,
                              AddressLine2 = r.AddressLine2,
                              PhoneNo = r.PhoneNo,
                              Email = r.Email,
                              TPIN = r.TPIN,
                              NAPSAAccountNumber = r.NAPSAAccountNumber,
                              NHIMAAccountNumber = r.NHIMAAccountNumber,
                              BankAccountNumber = r.BankAccountNumber,
                              DbConnectionString = r.DbConnectionString,
                              BankId = r.BankId,
                              BranchId = r.BranchId,
                              BranchCode = r.BranchCode,
                              FirstPayDay = r.FirstPayday,
                              Currency = r.Currency,
                              NumberOfWorkingDaysPerPayPeriod = r.NumberOfWorkingDaysPerPayPeriod,
                              PayFrequency = r.PayFrequency,
                              RegularHours = r.RegularHours,
                              AllowNegativePay = r.AllowNegativePay,
                              LastChanged = r.LastChanged!,
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("Organisation");
            return result;
        }

        public void Insert(OrganisationDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new OrganisationEntity
            {
                Id = data.Id,
                Name = data.Name,
                Logo = data.Logo,
                Country = data.Country,
                AddressLine1 = data.AddressLine1,
                AddressLine2 = data.AddressLine2,
                PhoneNo = data.PhoneNo,
                Email = data.Email,
                TPIN = data.TPIN,
                NAPSAAccountNumber = data.NAPSAAccountNumber,
                NHIMAAccountNumber = data.NHIMAAccountNumber,
                BankAccountNumber = data.BankAccountNumber,
                DbConnectionString = data.DbConnectionString,
                BankId = data.BankId,
                BranchId = data.BranchId,
                BranchCode = data.BranchCode,
                FirstPayDay = data.FirstPayday,
                Currency = data.Currency,
                NumberOfWorkingDaysPerPayPeriod = data.NumberOfWorkingDaysPerPayPeriod,
                PayFrequency = data.PayFrequency,
                RegularHours = data.RegularHours,
                AllowNegativePay = data.AllowNegativePay,
                LastChanged = data.LastChanged!,
            };
            MockDb.Organisations.Add(newItem);
        }
    }
}
