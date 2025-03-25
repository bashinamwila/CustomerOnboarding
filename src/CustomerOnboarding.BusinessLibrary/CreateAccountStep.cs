using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csla.Core;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class CreateAccountStep :
        StepBase<CreateAccountStep>
    {

         public static readonly PropertyInfo<string> RuleSetProperty =
            RegisterProperty<string>(nameof(RuleSet));
        public string RuleSet
        {
            get => GetProperty(RuleSetProperty);
            private set => LoadProperty(RuleSetProperty, value);
        }

       
    public static readonly PropertyInfo<Organisation> OrganisationProperty =
    RegisterProperty<Organisation>(nameof(Organisation));


        public Organisation Organisation
        {
            get=> GetProperty(OrganisationProperty);
            private set => LoadProperty(OrganisationProperty, value);
        }




        public static readonly PropertyInfo<User> UserProperty =
          RegisterProperty<User>(nameof(User));
        public User User
        {
            get => GetProperty(UserProperty);
            private set => LoadProperty(UserProperty, value);
        }

       public static readonly PropertyInfo<byte[]> TimeStampProperty =
           RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get => GetProperty(TimeStampProperty);
            set => SetProperty(TimeStampProperty, value);
        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            base.OnChildChanged(e);
            if (e.ChildObject is Organisation || e.ChildObject is User)
            {
                BusinessRules.CheckRules();
            }
        }


        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.AddRule(new CheckIfStepIsComplete(OrganisationProperty,IsCompletedProperty));
            BusinessRules.AddRule(new CheckIfStepIsComplete(UserProperty, IsCompletedProperty));


        }

        [CreateChild]
        private async Task CreateAsync(string tenantId,int id, [Inject] IStepTypeDal dal,
            [Inject]IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(id);
                Id = data.Id;
                Name = data.Name;
                RuleSet = data.RuleSet;
                Type = (StepType)Enum.Parse(typeof(StepType), data.Type.ToString());
                StepIndex = 0;
                IsCompleted = false;
                Organisation=await portal.GetPortal<Organisation>().CreateChildAsync(tenantId,RuleSet);
                User = await portal.GetPortal<User>().CreateChildAsync();

            }
            BusinessRules.CheckRules();
        }

        [FetchChild]
        private async Task FetchAsync(string tenantId, int id, [Inject] ICreateAccountStepDal dal,
            [Inject]IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId, id);
                Id = data.StepId;
                Name = data.Name;
                RuleSet = data.RuleSet;
                Type =(StepType)Enum.Parse(typeof(StepType), data.Type.ToString());
                TimeStamp = data.LastChanged;
                Organisation = await portal.GetPortal<Organisation>().FetchChildAsync(tenantId, RuleSet);
                User = await portal.GetPortal<User>().FetchChildAsync(tenantId);
            }
            BusinessRules.CheckRules();
        }

        [InsertChild]
        private async Task InsertAsync(CustomerOnboardingOrchestrator parent,[Inject]ICreateAccountStepDal dal,
            [Inject]IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = new CreateAccountStepDto
                {
                    TenantId=parent.TenantId,
                    StepId=this.Id,
                    
                   
                };
                dal.Insert(data);
                TimeStamp = data.LastChanged;
                await portal.GetPortal<Organisation>().UpdateChildAsync(Organisation, parent);
                await portal.GetPortal<User>().UpdateChildAsync(User, parent);

            }
        }

        [UpdateChild]
        private void Update(CustomerOnboardingOrchestrator parent, [Inject] ICreateAccountStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = new CreateAccountStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    LastChanged =this.TimeStamp
                };
                 dal.Update(data);
                TimeStamp = data.LastChanged;
            }
        }
    }
}
