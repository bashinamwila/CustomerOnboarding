using Csla;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class BankList : ReadOnlyListBase<BankList, BankInfo>
    {
        public BankInfo? GetBank(string id)
        {
            var result = (from r in this
                          where r.Id == id
                          select r).FirstOrDefault();
            return result;
        }
        public bool Contains(string id)
        {
            var result = (from r in this
                          where r.Id == id
                          select r.Id).Count();
            return result > 0;
        }

        

          [Fetch]
          private void Fetch([Inject] IBankDal dal,
              [Inject]IChildDataPortal<BankInfo>portal)
          {
              var rlce = this.RaiseListChangedEvents;
              this.RaiseListChangedEvents = false;
              this.IsReadOnly = false;
              var list = dal.Fetch();
              foreach (var data in list)
                  this.Add(portal.FetchChild(data));
              this.IsReadOnly = true;
              this.RaiseListChangedEvents = rlce;
          }
        
    /*
        [Fetch]
        private async Task FetchAsync([Inject] HttpClient httpClient,
            [Inject] IChildDataPortal<BankInfo> portal)
        {
            try
            {
                var response = await httpClient.GetAsync("https://localhost:7176/api/Bank/GetBanks");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var bankDtos = JsonConvert.DeserializeObject<List<BankModel>>(content);
                    IsReadOnly = false;
                    foreach (var bankDto in bankDtos!)
                    {
                        this.Add(portal.FetchChild(bankDto));

                    }
                    IsReadOnly = true;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("An error occurred on the server while fetching banks.");
                }
                else
                {
                    throw new Exception($"Failed to fetch banks. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new DataPortalException($"Error fetching banks: {ex.Message}", ex);
            }
        }
    */

    }
}
