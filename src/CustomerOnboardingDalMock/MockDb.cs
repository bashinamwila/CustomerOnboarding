using CustomerOnboarding.DalMock.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public static class MockDb
    {
        public static List<StepEntity> Steps { get; private set; } = default!;
        public static List<UserOnboardingEntity> UserOnboardingWorkflows { get; private set; } = default!;

        public static List<TenantOnboardingEntity> TenantOnboardingWorkflows { get; private set; } = default!;
        public static List<CreateAccountStepEntity>CreateAccounts { get; private set; }=default!;
        public static List<SendEmailNotificationStepEntity> SendEmailNotifications { get; private set; } = default!;
        public static List<OrganisationEntity> Organisations { get; private set; } = default!;
        public static List<UserEntity> Users { get; private set; } = default!;
        public static List<CountryEntity> Countries { get; private set; } = default!;
        public static List<EmailTemplateEntity> EmailTemplates { get; private set; }
        public static List<ConfirmEmailStepEntity> EmailConfirmations { get; private set;} = default!;
        public static List<OrganisationProfileStepEntity> OrganisationProfileSteps { get; private set; } = default!;
        public static List<OrganisationProfileEntity> OrganisationProfiles { get; private set; } = default!;
        public static List<BankingDetailsStepEntity> BankingDetailsSteps { get; private set; } = default!;
        public static List<BankingDetailsEntity> BankingDetails { get; private set; } = default!;
        public static List<BankEntity> Banks { get; private set; } = default!;

        public static List<BranchEntity> Branches { get; private set; } = default!;
        static MockDb()
        {
            Steps = new List<StepEntity>
            {
                new StepEntity{Id=1,Name="Create Account",Type=1,FullTypeName="CustomerOnboarding.BusinessLibrary.CreateAccountStep,CustomerOnboarding.BusinessLibrary",RuleSet="Create Account",LastChanged=GetTimeStamp()},
                new StepEntity{Id=2,Name="Send Email Notification",Type=2,FullTypeName="CustomerOnboarding.BusinessLibrary.SendEmailNotificationStep,CustomerOnboarding.BusinessLibrary",LastChanged=GetTimeStamp()},
                new StepEntity{Id=3,Name="Confirm Account",Type=1,FullTypeName="CustomerOnboarding.BusinessLibrary.ConfirmEmailStep,CustomerOnboarding.BusinessLibrary",RuleSet="Confirm Email", LastChanged = GetTimeStamp()},
                new StepEntity{Id=4,Name="Organisation Profile",Type=1,FullTypeName="CustomerOnboarding.BusinessLibrary.OrganisationProfileStep,CustomerOnboarding.BusinessLibrary",RuleSet="Organisation Profile", LastChanged = GetTimeStamp()},
                new StepEntity{Id=5,Name="Banking Details",Type=1,FullTypeName="CustomerOnboarding.BusinessLibrary.BankingDetailsStep,CustomerOnboarding.BusinessLibrary",RuleSet="Banking Details", LastChanged = GetTimeStamp()}

            };
            UserOnboardingWorkflows = new List<UserOnboardingEntity>();
            TenantOnboardingWorkflows = new();
            CreateAccounts = new List<CreateAccountStepEntity>();
            SendEmailNotifications = new List<SendEmailNotificationStepEntity>();
            EmailConfirmations = new List<ConfirmEmailStepEntity>();
            Organisations =new();
            OrganisationProfileSteps = new();
            OrganisationProfiles = new();
            Users = new();
            BankingDetailsSteps = new();
            BankingDetails = new(); 
            Countries = new List<CountryEntity>
            {
                new CountryEntity
                {
                    Id = "ZM",
                    Name="Zambia",
                    LastChanged=GetTimeStamp()
                },
                new CountryEntity
                {
                    Id = "ZA",
                    Name="South Africa",
                    LastChanged=GetTimeStamp()
                },
                new CountryEntity
                {
                    Id = "NG",
                    Name="Nigeria",
                    LastChanged=GetTimeStamp()
                },
                new CountryEntity
                { Id = "KR",
                Name="Kenya",
                LastChanged=GetTimeStamp()
                },
                new CountryEntity
                {
                    Id = "GH",
                    Name="Ghana",
                    LastChanged=GetTimeStamp()
                }


            };
            EmailTemplates = new List<EmailTemplateEntity>
            {
                new EmailTemplateEntity
                {
                    Id=1,
                    TemplateName="ConfirmEmailTemplate",
                    AssemblyQualifiedName="CustomerOnboarding.BusinessLibrary.Templates.Email.EmailConfirmationTemplate,CustomerOnboarding.BusinessLibrary",
                    LastChanged=GetTimeStamp()
                }
            };

        Banks= new List<BankEntity>
         {
                new BankEntity
                {
                    BankId="02",
                    Name="Absa Bank Zambia PLC",
                    SwiftCode="XXXX"

                },
                new BankEntity
                {
                    BankId="35",
                    Name="Access Bank Zambia Limited",
                     SwiftCode="XXXX"

                },

                new BankEntity
                {
                    BankId="19",
                    Name="Bank of China Zambia Limited",
                     SwiftCode="XXXX"

                },
                new BankEntity
                {
                    BankId="03",
                    Name="Citibank Zambia Limited",
                     SwiftCode="XXXX"

                },
                new BankEntity
                {
                    BankId="36",
                    Name="Ecobank Zambia Limited"

                },
                new BankEntity
                {
                    BankId="34",
                    Name="First Alliance Zambia Limited",
                     SwiftCode="XXXX"

                },
                new BankEntity
                {
                    BankId="28",
                    Name="First Capital Bank Zambia Limited",
                     SwiftCode="XXXX"

                },
                new BankEntity
                {
                    BankId="26",
                    Name="First National Bank Zambia Limited",
                     SwiftCode="XXXX"

                },
                new BankEntity
                {
                    BankId="09",
                    Name="Indo-Zambia Bank Zambia Limited",
                     SwiftCode="XXXX"

                },

                new BankEntity
                {
                    BankId="04",
                    Name="Stanbic Bank Zambia Limited",
                    SwiftCode="XXXX"

                },
                new BankEntity
                {
                    BankId="06",
                    Name="Standard Chartered Bank Zambia PLC",
                    SwiftCode="XXXX"

                },
                new BankEntity
                {
                    BankId="37",
                    Name="United Bank for Africa Zambia Limited",
                    SwiftCode="XXXX"

                },
                new BankEntity
                {
                    BankId="15",
                    Name="Zambia Industrial Commercial Bank Limited",
                    SwiftCode="XXXX"

                },
                 new BankEntity
                {
                    BankId="01",
                    Name="Zambia National Commercial Bank PLC",
                    SwiftCode="XXXX"

                },
                   new BankEntity
                {
                    BankId="21",
                    Name="AB Bank Zambia Limited",
                    SwiftCode="XXXX"

                }


            };

            Branches = new List<BranchEntity>
            {


                        new BranchEntity
                        {
                            Id = 1,
                            Name = "International Banking",
                            BankId = "01",
                            BranchCode = "0102"
                        },
                        new BranchEntity
                        {
                            Id = 2,
                            Name = "Lusaka Business Centre",
                            BankId = "01",
                            BranchCode = "0103"
                        },
                        new BranchEntity
                        {
                            Id = 3,
                            Name = "Kawambwa",
                            BankId = "01",
                            BranchCode = "0104"
                        },
                        new BranchEntity
                        {
                            Id = 4,
                            Name = "Petauke",
                            BankId = "01",
                            BranchCode = "0105"
                        },
                        new BranchEntity
                        {
                            Id = 5,
                            Name = "Mfuwe",
                            BankId = "01",
                            BranchCode = "0106"
                        },
                        new BranchEntity
                        {
                            Id = 6,
                            Name = "Human Resources",
                            BankId = "01",
                            BranchCode = "0107"
                        },
                        new BranchEntity
                        {
                            Id = 7,
                            Name = "Chisamba",
                            BankId = "01",
                            BranchCode = "0108"
                        },
                        new BranchEntity
                        {
                            Id = 8,
                            Name = "Mkushi",
                            BankId = "01",
                            BranchCode = "0109"
                        },
                        new BranchEntity
                        {
                            Id = 9,
                            Name = "Head Office Processing Centre",
                            BankId = "01",
                            BranchCode = "0116"
                        },
                        new BranchEntity
                        {
                            Id = 10,
                            Name = "Kitwe Clearing Centre",
                            BankId = "01",
                            BranchCode = "0117"
                        },
                        new BranchEntity
                        {
                            Id = 11,
                            Name = "Treasury",
                            BankId = "01",
                            BranchCode = "0118"
                        },
                        new BranchEntity
                        {
                            Id = 12,
                            Name = "Cairo Business Centre",
                            BankId = "01",
                            BranchCode = "0140"
                        },
                        new BranchEntity
                        {
                            Id = 13,
                            Name = "Lusaka North end",
                            BankId = "01",
                            BranchCode = "0141"
                        },
                        new BranchEntity
                        {
                            Id = 14,
                            Name = "Ndola Business Centre",
                            BankId = "01",
                            BranchCode = "0142"
                        },
                        new BranchEntity
                        {
                            Id = 15,
                            Name = "Mufulira",
                            BankId = "01",
                            BranchCode = "0143"
                        },
                        new BranchEntity
                        {
                            Id = 16,
                            Name = "Livingstone",
                            BankId = "01",
                            BranchCode = "0144"
                        },
                        new BranchEntity
                        {
                            Id = 17,
                            Name = "Kitwe Obote",
                            BankId = "01",
                            BranchCode = "0145"
                        },
                        new BranchEntity
                        {
                            Id = 18,
                            Name = "Kabwe",
                            BankId = "01",
                            BranchCode = "0146"
                        },
                        new BranchEntity
                        {
                            Id = 19,
                            Name = "Mazabuka",
                            BankId = "01",
                            BranchCode = "0147"
                        },
                        new BranchEntity
                        {
                            Id = 20,
                            Name = "Mansa",
                            BankId = "01",
                            BranchCode = "0148"
                        },
                        new BranchEntity
                        {
                            Id = 21,
                            Name = "Chingola",
                            BankId = "01",
                            BranchCode = "0149"
                        },
                        new BranchEntity
                        {
                            Id = 22,
                            Name = "Government Business Centre",
                            BankId = "01",
                            BranchCode = "0150"
                        },
                        new BranchEntity
                        {
                            Id = 23,
                            Name = "Mongu",
                            BankId = "01",
                            BranchCode = "0151"
                        },
                        new BranchEntity
                        {
                            Id = 24,
                            Name = "Lusaka Centre",
                            BankId = "01",
                            BranchCode = "0152"
                        },
                        new BranchEntity
                        {
                            Id = 25,
                            Name = "Lusaka Kwacha",
                            BankId = "01",
                            BranchCode = "0153"
                        },
                        new BranchEntity
                        {
                            Id = 26,
                            Name = "Ndola West",
                            BankId = "01",
                            BranchCode = "0154"
                        },
                        new BranchEntity
                        {
                            Id = 27,
                            Name = "Debt Recovery",
                            BankId = "01",
                            BranchCode = "0155"
                        },
                        new BranchEntity
                        {
                            Id = 28,
                            Name = "Kitwe Industrial",
                            BankId = "01",
                            BranchCode = "0156"
                        },
                        new BranchEntity
                        {
                            Id = 29,
                            Name = "Monze",
                            BankId = "01",
                            BranchCode = "0157"
                        },
                        new BranchEntity
                        {
                            Id = 30,
                            Name = "Kafue",
                            BankId = "01",
                            BranchCode = "0158"
                        },
                        new BranchEntity
                        {
                            Id = 31,
                            Name = "Choma",
                            BankId = "01",
                            BranchCode = "0159"
                        },
                        new BranchEntity
                        {
                            Id = 32,
                            Name = "Chipata",
                            BankId = "01",
                            BranchCode = "0160"
                        },
                        new BranchEntity
                        {
                            Id = 33,
                            Name = "Kapiri Mposhi",
                            BankId = "01",
                            BranchCode = "0161"
                        },
                        new BranchEntity
                        {
                            Id = 34,
                            Name = "Kasama",
                            BankId = "01",
                            BranchCode = "0162"
                        },
                        new BranchEntity
                        {
                            Id = 35,
                            Name = "Luanshya",
                            BankId = "01",
                            BranchCode = "0163"
                        },
                        new BranchEntity
                        {
                            Id = 36,
                            Name = "Ndola Industrial",
                            BankId = "01",
                            BranchCode = "0164"
                        },
                        new BranchEntity
                        {
                            Id = 37,
                            Name = "Mpika",
                            BankId = "01",
                            BranchCode = "0165"
                        },
                        new BranchEntity
                        {
                            Id = 38,
                            Name = "Lusaka Premium House",
                            BankId = "01",
                            BranchCode = "0166"
                        },
                        new BranchEntity
                        {
                            Id = 39,
                            Name = "Lusaka Civic Centre",
                            BankId = "01",
                            BranchCode = "0167"
                        },
                        new BranchEntity
                        {
                            Id = 40,
                            Name = "Solwezi",
                            BankId = "01",
                            BranchCode = "0168"
                        },
                        new BranchEntity
                        {
                            Id = 41,
                            Name = "Siavonga",
                            BankId = "01",
                            BranchCode = "0169"
                        },
                        new BranchEntity
                        {
                            Id = 42,
                            Name = "Maamba",
                            BankId = "01",
                            BranchCode = "0170"
                        },
                        new BranchEntity
                        {
                            Id = 43,
                            Name = "Lundazi",
                            BankId = "01",
                            BranchCode = "0171"
                        },
                        new BranchEntity
                        {
                            Id = 44,
                            Name = "Namwala",
                            BankId = "01",
                            BranchCode = "0172"
                        },
                        new BranchEntity
                        {
                            Id = 45,
                            Name = "Twin Palms Mall",
                            BankId = "01",
                            BranchCode = "0173"
                        },
                        new BranchEntity
                        {
                            Id = 46,
                            Name = "Lusaka City Market",
                            BankId = "01",
                            BranchCode = "0174"
                        },
                        new BranchEntity
                        {
                            Id = 47,
                            Name = "North mead",
                            BankId = "01",
                            BranchCode = "0175"
                        },
                        new BranchEntity
                        {
                            Id = 48,
                            Name = "Manda Hill",
                            BankId = "01",
                            BranchCode = "0178"
                        },
                        new BranchEntity
                        {
                            Id = 49,
                            Name = "Itezhi-Tezhi",
                            BankId = "01",
                            BranchCode = "0179"
                        },
                        new BranchEntity
                        {
                            Id = 50,
                            Name = "Senanga",
                            BankId = "01",
                            BranchCode = "0181"
                        },
                        new BranchEntity
                        {
                            Id = 51,
                            Name = "Chirundu",
                            BankId = "01",
                            BranchCode = "0182"
                        },
                        new BranchEntity
                        {
                            Id = 52,
                            Name = "Xapit",
                            BankId = "01",
                            BranchCode = "0183"
                        },
                        new BranchEntity
                        {
                            Id = 53,
                            Name = "Government Complex",
                            BankId = "01",
                            BranchCode = "0184"
                        },
                        new BranchEntity
                        {
                            Id = 54,
                            Name = "Woodlands",
                            BankId = "01",
                            BranchCode = "0185"
                        },
                        new BranchEntity
                        {
                            Id = 55,
                            Name = "Acacia Park Branch",
                            BankId = "01",
                            BranchCode = "0186"
                        },
                        new BranchEntity
                        {
                            Id = 56,
                            Name = "Nakonde",
                            BankId = "01",
                            BranchCode = "0196"
                        },
                        new BranchEntity
                        {
                            Id = 57,
                            Name = "Mukuba branch",
                            BankId = "01",
                            BranchCode = "0198"
                        },
                        new BranchEntity
                        {
                            Id = 58,
                            Name = "Chinsali Branch",
                            BankId = "01",
                            BranchCode = "0107"
                        },
                        new BranchEntity
                        {
                            Id = 59,
                            Name = "Waterfalls",
                            BankId = "01",
                            BranchCode = "0199"
                        },
                        new BranchEntity
                        {
                            Id = 60,
                            Name = "Digital",
                            BankId = "01",
                            BranchCode = "0193"
                        },
                        new BranchEntity
                        {
                            Id = 61,
                            Name = "Head Office -Elunda",
                            BankId = "02",
                            BranchCode = "0202"
                        },
                        new BranchEntity
                        {
                            Id = 62,
                            Name = "Chingola & Chingola Prestige",
                            BankId = "02",
                            BranchCode = "0203"
                        },
                        new BranchEntity
                        {
                            Id = 63,
                            Name = "Chipata",
                            BankId = "02",
                            BranchCode = "0204"
                        },
                        new BranchEntity
                        {
                            Id = 64,
                            Name = "Choma",
                            BankId = "02",
                            BranchCode = "0205"
                        },
                        new BranchEntity
                        {
                            Id = 65,
                            Name = "Kabwe",
                            BankId = "02",
                            BranchCode = "0206"
                        },
                        new BranchEntity
                        {
                            Id = 66,
                            Name = "Kafue",
                            BankId = "02",
                            BranchCode = "0207"
                        },
                        new BranchEntity
                        {
                            Id = 67,
                            Name = "Lusaka - Kamwala",
                            BankId = "02",
                            BranchCode = "0208"
                        },
                        new BranchEntity
                        {
                            Id = 68,
                            Name = "Kitwe Business Centre",
                            BankId = "02",
                            BranchCode = "0209"
                        },
                        new BranchEntity
                        {
                            Id = 69,
                            Name = "Kitwe Chimwemwe",
                            BankId = "02",
                            BranchCode = "0210"
                        },
                        new BranchEntity
                        {
                            Id = 70,
                            Name = "Kapiri Mposhi",
                            BankId = "02",
                            BranchCode = "0211"
                        },
                        new BranchEntity
                        {
                            Id = 71,
                            Name = "Livingstone & Livingstone Prestige",
                            BankId = "02",
                            BranchCode = "0212"
                        },
                        new BranchEntity
                        {
                            Id = 72,
                            Name = "Luanshya",
                            BankId = "02",
                            BranchCode = "0213"
                        },
                        new BranchEntity
                        {
                            Id = 73,
                            Name = "Lusaka Northend",
                            BankId = "02",
                            BranchCode = "0214"
                        },
                        new BranchEntity
                        {
                            Id = 74,
                            Name = "Lusaka - Matero",
                            BankId = "02",
                            BranchCode = "0215"
                        },
                        new BranchEntity
                        {
                            Id = 75,
                            Name = "Lusaka Business Centre",
                            BankId = "02",
                            BranchCode = "0216"
                        },
                        new BranchEntity
                        {
                            Id = 76,
                            Name = "Lusaka Longacres & Prestige",
                            BankId = "02",
                            BranchCode = "0217"
                        },
                        new BranchEntity
                        {
                            Id = 77,
                            Name = "Chilenje",
                            BankId = "02",
                            BranchCode = "0218"
                        },
                        new BranchEntity
                        {
                            Id = 78,
                            Name = "Lusaka - Industrial",
                            BankId = "02",
                            BranchCode = "0219"
                        },
                        new BranchEntity
                        {
                            Id = 79,
                            Name = "Mansa",
                            BankId = "02",
                            BranchCode = "0220"
                        },
                        new BranchEntity
                        {
                            Id = 80,
                            Name = "Mazabuka",
                            BankId = "02",
                            BranchCode = "0221"
                        },
                        new BranchEntity
                        {
                            Id = 81,
                            Name = "Mfuwe",
                            BankId = "02",
                            BranchCode = "0222"
                        },
                        new BranchEntity
                        {
                            Id = 82,
                            Name = "Mufulira",
                            BankId = "02",
                            BranchCode = "0223"
                        },
                        new BranchEntity
                        {
                            Id = 83,
                            Name = "Monze",
                            BankId = "02",
                            BranchCode = "0224"
                        },
                        new BranchEntity
                        {
                            Id = 84,
                            Name = "Ndola Business Centre",
                            BankId = "02",
                            BranchCode = "0225"
                        },
                        new BranchEntity
                        {
                            Id = 85,
                            Name = "University of Zambia Lusaka",
                            BankId = "02",
                            BranchCode = "0226"
                        },
                        new BranchEntity
                        {
                            Id = 86,
                            Name = "Kalomo",
                            BankId = "02",
                            BranchCode = "0227"
                        },
                        new BranchEntity
                        {
                            Id = 87,
                            Name = "Katete",
                            BankId = "02",
                            BranchCode = "0228"
                        },
                        new BranchEntity
                        {
                            Id = 88,
                            Name = "Solwezi",
                            BankId = "02",
                            BranchCode = "0229"
                        },
                        new BranchEntity
                        {
                            Id = 89,
                            Name = "Petauke",
                            BankId = "02",
                            BranchCode = "0230"
                        },
                        new BranchEntity
                        {
                            Id = 90,
                            Name = "Lundazi",
                            BankId = "02",
                            BranchCode = "0231"
                        },
                        new BranchEntity
                        {
                            Id = 91,
                            Name = "Kasama",
                            BankId = "02",
                            BranchCode = "0232"
                        },
                        new BranchEntity
                        {
                            Id = 92,
                            Name = "Lusaka - Soweto",
                            BankId = "02",
                            BranchCode = "0233"
                        },
                        new BranchEntity
                        {
                            Id = 93,
                            Name = "Mumbwa",
                            BankId = "02",
                            BranchCode = "0234"
                        },
                        new BranchEntity
                        {
                            Id = 94,
                            Name = "Mongu",
                            BankId = "02",
                            BranchCode = "0235"
                        },
                        new BranchEntity
                        {
                            Id = 95,
                            Name = "Lusaka Chelston & Airport Agency",
                            BankId = "02",
                            BranchCode = "0236"
                        },
                        new BranchEntity
                        {
                            Id = 96,
                            Name = "Chongwe",
                            BankId = "02",
                            BranchCode = "0237"
                        },
                        new BranchEntity
                        {
                            Id = 97,
                            Name = "Mkushi",
                            BankId = "02",
                            BranchCode = "0238"
                        },
                        new BranchEntity
                        {
                            Id = 98,
                            Name = "Ndola Operations Processing Centre",
                            BankId = "02",
                            BranchCode = "0239"
                        },
                        new BranchEntity
                        {
                            Id = 99,
                            Name = "Nakonde",
                            BankId = "02",
                            BranchCode = "0240"
                        },
                        new BranchEntity
                        {
                            Id = 100,
                            Name = "Kitwe Parklands Center",
                            BankId = "02",
                            BranchCode = "0241"
                        },
                        new BranchEntity
                        {
                            Id = 101,
                            Name = "Chirundu",
                            BankId = "02",
                            BranchCode = "0242"
                        },
                        new BranchEntity
                        {
                            Id = 102,
                            Name = "Kabwata",
                            BankId = "02",
                            BranchCode = "0243"
                        },
                        new BranchEntity
                        {
                            Id = 103,
                            Name = "Lusaka - Chawama",
                            BankId = "02",
                            BranchCode = "0244"
                        },
                        new BranchEntity
                        {
                            Id = 104,
                            Name = "Mpika",
                            BankId = "02",
                            BranchCode = "0245"
                        },
                        new BranchEntity
                        {
                            Id = 105,
                            Name = "Ndola - Masala",
                            BankId = "02",
                            BranchCode = "0246"
                        },
                        new BranchEntity
                        {
                            Id = 106,
                            Name = "Chambishi",
                            BankId = "02",
                            BranchCode = "0247"
                        },
                        new BranchEntity
                        {
                            Id = 107,
                            Name = "Kalulushi",
                            BankId = "02",
                            BranchCode = "0248"
                        },
                        new BranchEntity
                        {
                            Id = 108,
                            Name = "Lusaka Operations Processing Centre",
                            BankId = "02",
                            BranchCode = "0250"
                        },
                        new BranchEntity
                        {
                            Id = 109,
                            Name = "Mbala",
                            BankId = "02",
                            BranchCode = "0251"
                        },
                        new BranchEntity
                        {
                            Id = 110,
                            Name = "Kitwe Operations Processing Centre",
                            BankId = "02",
                            BranchCode = "0252"
                        },
                        new BranchEntity
                        {
                            Id = 111,
                            Name = "Chililabombwe",
                            BankId = "02",
                            BranchCode = "0253"
                        },
                        new BranchEntity
                        {
                            Id = 112,
                            Name = "Lusaka Kabelenga",
                            BankId = "02",
                            BranchCode = "0254"
                        },
                        new BranchEntity
                        {
                            Id = 113,
                            Name = "Elunda Premium Banking Centre",
                            BankId = "02",
                            BranchCode = "0255"
                        },
                        new BranchEntity
                        {
                            Id = 114,
                            Name = "Manda Hill",
                            BankId = "02",
                            BranchCode = "0249"
                        },
                        new BranchEntity
                        {
                            Id = 115,
                            Name = "Lusaka",
                            BankId = "03",
                            BranchCode = "0301"
                        },
                        new BranchEntity
                        {
                            Id = 116,
                            Name = "Ndola",
                            BankId = "03",
                            BranchCode = "0302"
                        },
                        new BranchEntity
                        {
                            Id = 117,
                            Name = "Mcommerce Branch",
                            BankId = "03",
                            BranchCode = "0303"
                        },
                        new BranchEntity
                        {
                            Id = 118,
                            Name = "Citibank Natsave",
                            BankId = "03",
                            BranchCode = "0307"
                        },
                        new BranchEntity
                        {
                            Id = 119,
                            Name = "Citibank ZNBS Branch",
                            BankId = "03",
                            BranchCode = "0308"
                        },
                        new BranchEntity
                        {
                            Id = 120,
                            Name = "Permanent House",
                            BankId = "03",
                            BranchCode = "0331"
                        },
                        new BranchEntity
                        {
                            Id = 121,
                            Name = "Soweto Agency",
                            BankId = "03",
                            BranchCode = "0333"
                        },
                        new BranchEntity
                        {
                            Id = 122,
                            Name = "Nyimba",
                            BankId = "03",
                            BranchCode = "0334"
                        },
                        new BranchEntity
                        {
                            Id = 123,
                            Name = "Society House",
                            BankId = "03",
                            BranchCode = "0332"
                        },
                        new BranchEntity
                        {
                            Id = 124,
                            Name = "Ndola Branch",
                            BankId = "03",
                            BranchCode = "0337"
                        },
                        new BranchEntity
                        {
                            Id = 125,
                            Name = "Kitwe",
                            BankId = "03",
                            BranchCode = "0338"
                        },
                        new BranchEntity
                        {
                            Id = 126,
                            Name = "Kasama",
                            BankId = "03",
                            BranchCode = "0346"
                        },
                        new BranchEntity
                        {
                            Id = 127,
                            Name = "Chipata",
                            BankId = "03",
                            BranchCode = "0349"
                        },
                        new BranchEntity
                        {
                            Id = 128,
                            Name = "Livingstone",
                            BankId = "03",
                            BranchCode = "0350"
                        },
                        new BranchEntity
                        {
                            Id = 129,
                            Name = "Choma",
                            BankId = "03",
                            BranchCode = "0364"
                        },
                        new BranchEntity
                        {
                            Id = 130,
                            Name = "Kabwe",
                            BankId = "03",
                            BranchCode = "0365"
                        },
                        new BranchEntity
                        {
                            Id = 131,
                            Name = "Chingola",
                            BankId = "03",
                            BranchCode = "0366"
                        },
                        new BranchEntity
                        {
                            Id = 132,
                            Name = "Luanshya",
                            BankId = "03",
                            BranchCode = "0367"
                        },
                        new BranchEntity
                        {
                            Id = 133,
                            Name = "Mufulira",
                            BankId = "03",
                            BranchCode = "0368"
                        },
                        new BranchEntity
                        {
                            Id = 134,
                            Name = "Chililabombwe",
                            BankId = "03",
                            BranchCode = "0369"
                        },
                        new BranchEntity
                        {
                            Id = 135,
                            Name = "Mansa",
                            BankId = "03",
                            BranchCode = "0370"
                        },
                        new BranchEntity
                        {
                            Id = 136,
                            Name = "Solwezi",
                            BankId = "03",
                            BranchCode = "0371"
                        },
                        new BranchEntity
                        {
                            Id = 137,
                            Name = "Mongu",
                            BankId = "03",
                            BranchCode = "0372"
                        },
                        new BranchEntity
                        {
                            Id = 138,
                            Name = "Mazabuka",
                            BankId = "03",
                            BranchCode = "0373"
                        },
                        new BranchEntity
                        {
                            Id = 139,
                            Name = "Mpika",
                            BankId = "03",
                            BranchCode = "0374"
                        },
                        new BranchEntity
                        {
                            Id = 140,
                            Name = "Kapiri Mposhi",
                            BankId = "03",
                            BranchCode = "0387"
                        },
                        new BranchEntity
                        {
                            Id = 141,
                            Name = "Head Office",
                            BankId = "04",
                            BranchCode = "0400"
                        },
                        new BranchEntity
                        {
                            Id = 142,
                            Name = "Lusaka",
                            BankId = "04",
                            BranchCode = "0402"
                        },
                        new BranchEntity
                        {
                            Id = 143,
                            Name = "Lusaka Industrial",
                            BankId = "04",
                            BranchCode = "0407"
                        },
                        new BranchEntity
                        {
                            Id = 144,
                            Name = "Mkushi",
                            BankId = "04",
                            BranchCode = "0408"
                        },
                        new BranchEntity
                        {
                            Id = 145,
                            Name = "Ndola Main",
                            BankId = "04",
                            BranchCode = "0403"
                        },
                        new BranchEntity
                        {
                            Id = 146,
                            Name = "Ndola South",
                            BankId = "04",
                            BranchCode = "0405"
                        },
                        new BranchEntity
                        {
                            Id = 147,
                            Name = "Kitwe",
                            BankId = "04",
                            BranchCode = "0406"
                        },
                        new BranchEntity
                        {
                            Id = 148,
                            Name = "Chingola",
                            BankId = "04",
                            BranchCode = "0409"
                        },
                        new BranchEntity
                        {
                            Id = 149,
                            Name = "Arcades",
                            BankId = "04",
                            BranchCode = "0410"
                        },
                        new BranchEntity
                        {
                            Id = 150,
                            Name = "Matero",
                            BankId = "04",
                            BranchCode = "0411"
                        },
                        new BranchEntity
                        {
                            Id = 151,
                            Name = "Solwezi",
                            BankId = "04",
                            BranchCode = "0412"
                        },
                        new BranchEntity
                        {
                            Id = 152,
                            Name = "Mazabuka",
                            BankId = "04",
                            BranchCode = "0413"
                        },
                        new BranchEntity
                        {
                            Id = 153,
                            Name = "Mufulira",
                            BankId = "04",
                            BranchCode = "0414"
                        },
                        new BranchEntity
                        {
                            Id = 154,
                            Name = "Mulungushi",
                            BankId = "04",
                            BranchCode = "0415"
                        },
                        new BranchEntity
                        {
                            Id = 155,
                            Name = "Chipata",
                            BankId = "04",
                            BranchCode = "0416"
                        },
                        new BranchEntity
                        {
                            Id = 156,
                            Name = "Livingstone",
                            BankId = "04",
                            BranchCode = "0417"
                        },
                        new BranchEntity
                        {
                            Id = 157,
                            Name = "Choma",
                            BankId = "04",
                            BranchCode = "0418"
                        },
                        new BranchEntity
                        {
                            Id = 158,
                            Name = "Lumwana",
                            BankId = "04",
                            BranchCode = "0421"
                        },
                        new BranchEntity
                        {
                            Id = 159,
                            Name = "Kabwe",
                            BankId = "04",
                            BranchCode = "0422"
                        },
                        new BranchEntity
                        {
                            Id = 160,
                            Name = "Soweto",
                            BankId = "04",
                            BranchCode = "0423"
                        },
                        new BranchEntity
                        {
                            Id = 161,
                            Name = "Chisokone",
                            BankId = "04",
                            BranchCode = "0424"
                        },
                        new BranchEntity
                        {
                            Id = 162,
                            Name = "Chambishi",
                            BankId = "04",
                            BranchCode = "0425"
                        },
                        new BranchEntity
                        {
                            Id = 163,
                            Name = "Private Banking",
                            BankId = "04",
                            BranchCode = "0427"
                        },
                        new BranchEntity
                        {
                            Id = 164,
                            Name = "Cosmopolitan Mall",
                            BankId = "04",
                            BranchCode = "0493"
                        },
                        new BranchEntity
                        {
                            Id = 165,
                            Name = "East Park Mall",
                            BankId = "04",
                            BranchCode = "0494"
                        },
                        new BranchEntity
                        {
                            Id = 166,
                            Name = "Kafubu Mall",
                            BankId = "04",
                            BranchCode = "0495"
                        },
                        new BranchEntity
                        {
                            Id = 167,
                            Name = "Mukuba Mall",
                            BankId = "04",
                            BranchCode = "0496"
                        },
                        new BranchEntity
                        {
                            Id = 168,
                            Name = "Kabulonga",
                            BankId = "04",
                            BranchCode = "0429"
                        },
                        new BranchEntity
                        {
                            Id = 169,
                            Name = "Woodlands",
                            BankId = "04",
                            BranchCode = "0430"
                        },
                        new BranchEntity
                        {
                            Id = 170,
                            Name = "Kabwata",
                            BankId = "04",
                            BranchCode = "0426"
                        },
                        new BranchEntity
                        {
                            Id = 171,
                            Name = "Waterfall",
                            BankId = "04",
                            BranchCode = "0439"
                        },
                        new BranchEntity
                        {
                            Id = 172,
                            Name = "Kafue Branch",
                            BankId = "04",
                            BranchCode = "0419"
                        },
                        new BranchEntity
                        {
                            Id = 173,
                            Name = "Kasama",
                            BankId = "06",
                            BranchCode = "0613"
                        },
                        new BranchEntity
                        {
                            Id = 174,
                            Name = "Kabulonga",
                            BankId = "06",
                            BranchCode = "0614"
                        },
                        new BranchEntity
                        {
                            Id = 175,
                            Name = "Cross Roads",
                            BankId = "06",
                            BranchCode = "0615"
                        },
                        new BranchEntity
                        {
                            Id = 176,
                            Name = "Lusaka Main",
                            BankId = "06",
                            BranchCode = "0617"
                        },
                        new BranchEntity
                        {
                            Id = 177,
                            Name = "Livingstone",
                            BankId = "06",
                            BranchCode = "0618"
                        },
                        new BranchEntity
                        {
                            Id = 178,
                            Name = "Mazabuka",
                            BankId = "06",
                            BranchCode = "0619"
                        },
                        new BranchEntity
                        {
                            Id = 179,
                            Name = "Jacaranda Mall Branch",
                            BankId = "06",
                            BranchCode = "0620"
                        },
                        new BranchEntity
                        {
                            Id = 180,
                            Name = "Levy Park Branch",
                            BankId = "06",
                            BranchCode = "0621"
                        },
                        new BranchEntity
                        {
                            Id = 181,
                            Name = "Zambia Way",
                            BankId = "06",
                            BranchCode = "0628"
                        },
                        new BranchEntity
                        {
                            Id = 182,
                            Name = "Manda Hill",
                            BankId = "06",
                            BranchCode = "0630"
                        },
                        new BranchEntity
                        {
                            Id = 183,
                            Name = "Luanshya",
                            BankId = "06",
                            BranchCode = "0632"
                        },
                        new BranchEntity
                        {
                            Id = 184,
                            Name = "Chingola",
                            BankId = "06",
                            BranchCode = "0636"
                        },
                        new BranchEntity
                        {
                            Id = 185,
                            Name = "Choma",
                            BankId = "06",
                            BranchCode = "0637"
                        },
                        new BranchEntity
                        {
                            Id = 186,
                            Name = "Mongu",
                            BankId = "06",
                            BranchCode = "0648"
                        },
                        new BranchEntity
                        {
                            Id = 187,
                            Name = "North end",
                            BankId = "06",
                            BranchCode = "0643"
                        },
                        new BranchEntity
                        {
                            Id = 188,
                            Name = "Chililabombwe",
                            BankId = "06",
                            BranchCode = "0644"
                        },
                        new BranchEntity
                        {
                            Id = 189,
                            Name = "Buteko",
                            BankId = "06",
                            BranchCode = "0671"
                        },
                        new BranchEntity
                        {
                            Id = 190,
                            Name = "Solwezi",
                            BankId = "06",
                            BranchCode = "0616"
                        },
                        new BranchEntity
                        {
                            Id = 191,
                            Name = "Lusaka Main",
                            BankId = "09",
                            BranchCode = "0901"
                        },
                        new BranchEntity
                        {
                            Id = 192,
                            Name = "Chilanga",
                            BankId = "09",
                            BranchCode = "0903"
                        },
                        new BranchEntity
                        {
                            Id = 193,
                            Name = "Kamwala",
                            BankId = "09",
                            BranchCode = "0904"
                        },
                        new BranchEntity
                        {
                            Id = 194,
                            Name = "North end",
                            BankId = "09",
                            BranchCode = "0905"
                        },
                        new BranchEntity
                        {
                            Id = 195,
                            Name = "Kabwe",
                            BankId = "09",
                            BranchCode = "0906"
                        },
                        new BranchEntity
                        {
                            Id = 196,
                            Name = "Ndola",
                            BankId = "09",
                            BranchCode = "0907"
                        },
                        new BranchEntity
                        {
                            Id = 197,
                            Name = "Kitwe",
                            BankId = "09",
                            BranchCode = "0908"
                        },
                        new BranchEntity
                        {
                            Id = 198,
                            Name = "Chingola",
                            BankId = "09",
                            BranchCode = "0909"
                        },
                        new BranchEntity
                        {
                            Id = 199,
                            Name = "Livingstone",
                            BankId = "09",
                            BranchCode = "0910"
                        },
                        new BranchEntity
                        {
                            Id = 200,
                            Name = "Lusaka Industrial",
                            BankId = "09",
                            BranchCode = "0911"
                        },
                        new BranchEntity
                        {
                            Id = 201,
                            Name = "Chipata",
                            BankId = "09",
                            BranchCode = "0912"
                        },
                        new BranchEntity
                        {
                            Id = 202,
                            Name = "Chawama",
                            BankId = "09",
                            BranchCode = "0913"
                        },
                        new BranchEntity
                        {
                            Id = 203,
                            Name = "Manda Hill Branch",
                            BankId = "09",
                            BranchCode = "0914"
                        },
                        new BranchEntity
                        {
                            Id = 204,
                            Name = "Nyimba Branch",
                            BankId = "09",
                            BranchCode = "0915"
                        },
                        new BranchEntity
                        {
                            Id = 205,
                            Name = "Chandwe Musonda",
                            BankId = "09",
                            BranchCode = "0916"
                        },
                        new BranchEntity
                        {
                            Id = 206,
                            Name = "Kasumbalesa Branch",
                            BankId = "09",
                            BranchCode = "0917"
                        },
                        new BranchEntity
                        {
                            Id = 207,
                            Name = "Choma Branch",
                            BankId = "09",
                            BranchCode = "0918"
                        },
                        new BranchEntity
                        {
                            Id = 208,
                            Name = "Solwezi",
                            BankId = "09",
                            BranchCode = "0919"
                        },
                        new BranchEntity
                        {
                            Id = 209,
                            Name = "Kasama",
                            BankId = "09",
                            BranchCode = "0920"
                        },
                        new BranchEntity
                        {
                            Id = 210,
                            Name = "Chinsali",
                            BankId = "09",
                            BranchCode = "0921"
                        },
                        new BranchEntity
                        {
                            Id = 211,
                            Name = "Jacaranda Mall",
                            BankId = "09",
                            BranchCode = "0922"
                        },
                        new BranchEntity
                        {
                            Id = 212,
                            Name = "Crossroads Shopping Mall",
                            BankId = "09",
                            BranchCode = "0923"
                        },
                        new BranchEntity
                        {
                            Id = 213,
                            Name = "Copperhill",
                            BankId = "09",
                            BranchCode = "0925"
                        },
                        new BranchEntity
                        {
                            Id = 214,
                            Name = "Mansa",
                            BankId = "09",
                            BranchCode = "0924"
                        },
                        new BranchEntity
                        {
                            Id = 215,
                            Name = "Mongu",
                            BankId = "09",
                            BranchCode = "0926"
                        },
                        new BranchEntity
                        {
                            Id = 216,
                            Name = "Kafue Branch",
                            BankId = "09",
                            BranchCode = "0927"
                        },
                        new BranchEntity
                        {
                            Id = 217,
                            Name = "Chilenje Branch",
                            BankId = "09",
                            BranchCode = "0928"
                        },
                        new BranchEntity
                        {
                            Id = 218,
                            Name = "Zimba Branch",
                            BankId = "09",
                            BranchCode = "0929"
                        },
                        new BranchEntity
                        {
                            Id = 219,
                            Name = "Serenje Branch",
                            BankId = "09",
                            BranchCode = "0930"
                        },
                        new BranchEntity
                        {
                            Id = 220,
                            Name = "Lundazi Agency",
                            BankId = "09",
                            BranchCode = "0931"
                        },
                        new BranchEntity
                        {
                            Id = 221,
                            Name = "Mungwi Agency",
                            BankId = "09",
                            BranchCode = "0932"
                        },
                        new BranchEntity
                        {
                            Id = 265,
                            Name = "Lusaka Business Centre",
                            BankId = "15",
                            BranchCode = "1501"
                        },
                        new BranchEntity
                        {
                            Id = 266,
                            Name = "Kitwe",
                            BankId = "15",
                            BranchCode = "1502"
                        },
                        new BranchEntity
                        {
                            Id = 297,
                            Name = "Lusaka",
                            BankId = "19",
                            BranchCode = "1901"
                        },
                        new BranchEntity
                        {
                            Id = 298,
                            Name = "Kitwe",
                            BankId = "19",
                            BranchCode = "1902"
                        },
                        new BranchEntity
                        {
                            Id = 299,
                            Name = "Main Cairo Road Branch",
                            BankId = "21",
                            BranchCode = "2101"
                        },
                        new BranchEntity
                        {
                            Id = 300,
                            Name = "Chilenje Branch",
                            BankId = "21",
                            BranchCode = "2102"
                        },
                        new BranchEntity
                        {
                            Id = 301,
                            Name = "Matero",
                            BankId = "21",
                            BranchCode = "2103"
                        },
                        new BranchEntity
                        {
                            Id = 302,
                            Name = "Kalingalinga Branch",
                            BankId = "21",
                            BranchCode = "2104"
                        },
                        new BranchEntity
                        {
                            Id = 303,
                            Name = "Chelston Branch",
                            BankId = "21",
                            BranchCode = "2105"
                        },
                        new BranchEntity
                        {
                            Id = 304,
                            Name = "Garden Branch",
                            BankId = "21",
                            BranchCode = "2106"
                        },
                        new BranchEntity
                        {
                            Id = 305,
                            Name = "Kitwe",
                            BankId = "21",
                            BranchCode = "2107"
                        },
                        new BranchEntity
                        {
                            Id = 306,
                            Name = "Commercial Suite Lusaka",
                            BankId = "26",
                            BranchCode = "2601"
                        },
                        new BranchEntity
                        {
                            Id = 307,
                            Name = "Industrial Branch",
                            BankId = "26",
                            BranchCode = "2602"
                        },
                        new BranchEntity
                        {
                            Id = 308,
                            Name = "Ndola Branch",
                            BankId = "26",
                            BranchCode = "2603"
                        },
                        new BranchEntity
                        {
                            Id = 309,
                            Name = "Head Office Lusaka",
                            BankId = "26",
                            BranchCode = "2605"
                        },
                        new BranchEntity
                        {
                            Id = 310,
                            Name = "Electronic Banking Branch",
                            BankId = "26",
                            BranchCode = "2606"
                        },
                        new BranchEntity
                        {
                            Id = 311,
                            Name = "Kitwe",
                            BankId = "26",
                            BranchCode = "2612"
                        },
                        new BranchEntity
                        {
                            Id = 312,
                            Name = "Mazabuka",
                            BankId = "26",
                            BranchCode = "2613"
                        },
                        new BranchEntity
                        {
                            Id = 313,
                            Name = "Manda Hill",
                            BankId = "26",
                            BranchCode = "2614"
                        },
                        new BranchEntity
                        {
                            Id = 314,
                            Name = "Makeni Mall",
                            BankId = "26",
                            BranchCode = "2616"
                        },
                        new BranchEntity
                        {
                            Id = 315,
                            Name = "Jacaranda Mall",
                            BankId = "26",
                            BranchCode = "2618"
                        },
                        new BranchEntity
                        {
                            Id = 316,
                            Name = "Mkushi",
                            BankId = "26",
                            BranchCode = "2619"
                        },
                        new BranchEntity
                        {
                            Id = 317,
                            Name = "Solwezi",
                            BankId = "26",
                            BranchCode = "2623"
                        },
                        new BranchEntity
                        {
                            Id = 318,
                            Name = "Chingola",
                            BankId = "26",
                            BranchCode = "2622"
                        },
                        new BranchEntity
                        {
                            Id = 319,
                            Name = "Chipata",
                            BankId = "26",
                            BranchCode = "2621"
                        },
                        new BranchEntity
                        {
                            Id = 320,
                            Name = "CIB (Corporate)",
                            BankId = "26",
                            BranchCode = "2629"
                        },
                        new BranchEntity
                        {
                            Id = 321,
                            Name = "Government and Public Sector",
                            BankId = "26",
                            BranchCode = "2636"
                        },
                        new BranchEntity
                        {
                            Id = 322,
                            Name = "Branchless Banking",
                            BankId = "26",
                            BranchCode = "2625"
                        },
                        new BranchEntity
                        {
                            Id = 323,
                            Name = "POS-Visa",
                            BankId = "26",
                            BranchCode = "2631"
                        },
                        new BranchEntity
                        {
                            Id = 324,
                            Name = "POS - MasterCard",
                            BankId = "26",
                            BranchCode = "2632"
                        },
                        new BranchEntity
                        {
                            Id = 325,
                            Name = "POS - FNB",
                            BankId = "26",
                            BranchCode = "2633"
                        },
                        new BranchEntity
                        {
                            Id = 326,
                            Name = "Kabwe Branch",
                            BankId = "26",
                            BranchCode = "2637"
                        },
                        new BranchEntity
                        {
                            Id = 327,
                            Name = "Choma Branch",
                            BankId = "26",
                            BranchCode = "2638"
                        },
                        new BranchEntity
                        {
                            Id = 328,
                            Name = "Premier Banking",
                            BankId = "26",
                            BranchCode = "2639"
                        },
                        new BranchEntity
                        {
                            Id = 329,
                            Name = "Agriculture Center",
                            BankId = "26",
                            BranchCode = "2640"
                        },
                        new BranchEntity
                        {
                            Id = 330,
                            Name = "Homes Loans",
                            BankId = "26",
                            BranchCode = "2620"
                        },
                        new BranchEntity
                        {
                            Id = 331,
                            Name = "Electronic Wallet",
                            BankId = "26",
                            BranchCode = "2627"
                        },
                        new BranchEntity
                        {
                            Id = 332,
                            Name = "Treasury Branch",
                            BankId = "26",
                            BranchCode = "2611"
                        },
                        new BranchEntity
                        {
                            Id = 333,
                            Name = "Vehicle and Asset Finance",
                            BankId = "26",
                            BranchCode = "2615"
                        },
                        new BranchEntity
                        {
                            Id = 334,
                            Name = "FNB Operations",
                            BankId = "26",
                            BranchCode = "2604"
                        },
                        new BranchEntity
                        {
                            Id = 335,
                            Name = "Luanshya",
                            BankId = "26",
                            BranchCode = "2641"
                        },
                        new BranchEntity
                        {
                            Id = 336,
                            Name = "Corporate Investment Banking",
                            BankId = "26",
                            BranchCode = "2642"
                        },
                        new BranchEntity
                        {
                            Id = 337,
                            Name = "Mukuba Mall Branch",
                            BankId = "26",
                            BranchCode = "2643"
                        },
                        new BranchEntity
                        {
                            Id = 338,
                            Name = "Mufulira Branch",
                            BankId = "26",
                            BranchCode = "2644"
                        },
                        new BranchEntity
                        {
                            Id = 339,
                            Name = "PHI Branch",
                            BankId = "26",
                            BranchCode = "2649"
                        },
                        new BranchEntity
                        {
                            Id = 340,
                            Name = "Chilenje Branch",
                            BankId = "26",
                            BranchCode = "2646"
                        },
                        new BranchEntity
                        {
                            Id = 341,
                            Name = "Kitwe Industrial",
                            BankId = "26",
                            BranchCode = "2647"
                        },
                        new BranchEntity
                        {
                            Id = 342,
                            Name = "Cash centre",
                            BankId = "26",
                            BranchCode = "2648"
                        },
                        new BranchEntity
                        {
                            Id = 343,
                            Name = "Cairo Road Branch",
                            BankId = "26",
                            BranchCode = "2650"
                        },
                        new BranchEntity
                        {
                            Id = 344,
                            Name = "Livingstone Branch",
                            BankId = "26",
                            BranchCode = "2661"
                        },
                        new BranchEntity
                        {
                            Id = 345,
                            Name = "Kabulonga Branch",
                            BankId = "26",
                            BranchCode = "2672"
                        },
                        new BranchEntity
                        {
                            Id = 346,
                            Name = "Kalumbila Branch",
                            BankId = "26",
                            BranchCode = "2627"
                        },
                        new BranchEntity
                        {
                            Id = 347,
                            Name = "Industrial Branch",
                            BankId = "28",
                            BranchCode = "2801"
                        },
                        new BranchEntity
                        {
                            Id = 348,
                            Name = "Cairo Branch",
                            BankId = "28",
                            BranchCode = "2802"
                        },
                        new BranchEntity
                        {
                            Id = 349,
                            Name = "Lusaka Main Branch",
                            BankId = "28",
                            BranchCode = "2803"
                        },
                        new BranchEntity
                        {
                            Id = 350,
                            Name = "Makeni Branch",
                            BankId = "28",
                            BranchCode = "2804"
                        },
                        new BranchEntity
                        {
                            Id = 351,
                            Name = "Ndola Branch",
                            BankId = "28",
                            BranchCode = "2805"
                        },
                        new BranchEntity
                        {
                            Id = 352,
                            Name = "Kamwala Branch",
                            BankId = "28",
                            BranchCode = "2806"
                        },
                        new BranchEntity
                        {
                            Id = 353,
                            Name = "Lusaka Main",
                            BankId = "34",
                            BranchCode = "3401"
                        },
                        new BranchEntity
                        {
                            Id = 354,
                            Name = "Ndola",
                            BankId = "34",
                            BranchCode = "3403"
                        },
                        new BranchEntity
                        {
                            Id = 355,
                            Name = "Kitwe",
                            BankId = "34",
                            BranchCode = "3404"
                        },
                        new BranchEntity
                        {
                            Id = 356,
                            Name = "Industrial Branch",
                            BankId = "34",
                            BranchCode = "3406"
                        },
                        new BranchEntity
                        {
                            Id = 357,
                            Name = "East Park Branch",
                            BankId = "34",
                            BranchCode = "3407"
                        },
                        new BranchEntity
                        {
                            Id = 358,
                            Name = "Northend",
                            BankId = "35",
                            BranchCode = "3501"
                        },
                        new BranchEntity
                        {
                            Id = 359,
                            Name = "Longacres",
                            BankId = "35",
                            BranchCode = "3502"
                        },
                        new BranchEntity
                        {
                            Id = 360,
                            Name = "Arcades",
                            BankId = "35",
                            BranchCode = "3503"
                        },
                        new BranchEntity
                        {
                            Id = 361,
                            Name = "Ndola Broadway",
                            BankId = "35",
                            BranchCode = "3504"
                        },
                        new BranchEntity
                        {
                            Id = 362,
                            Name = "Kitwe",
                            BankId = "35",
                            BranchCode = "3505"
                        },
                        new BranchEntity
                        {
                            Id = 363,
                            Name = "Makeni",
                            BankId = "35",
                            BranchCode = "3506"
                        },
                        new BranchEntity
                        {
                            Id = 364,
                            Name = "Thabo Mbeki",
                            BankId = "36",
                            BranchCode = "3602"
                        },
                        new BranchEntity
                        {
                            Id = 365,
                            Name = "Cairo Road",
                            BankId = "36",
                            BranchCode = "3603"
                        },
                        new BranchEntity
                        {
                            Id = 366,
                            Name = "Woodlands",
                            BankId = "36",
                            BranchCode = "3604"
                        },
                        new BranchEntity
                        {
                            Id = 367,
                            Name = "Kitwe",
                            BankId = "36",
                            BranchCode = "3605"
                        },
                        new BranchEntity
                        {
                            Id = 368,
                            Name = "Chibombo",
                            BankId = "36",
                            BranchCode = "3606"
                        },
                        new BranchEntity
                        {
                            Id = 369,
                            Name = "Industrial Branch",
                            BankId = "36",
                            BranchCode = "3607"
                        },
                        new BranchEntity
                        {
                            Id = 370,
                            Name = "Copperbelt University",
                            BankId = "36",
                            BranchCode = "3608"
                        },
                        new BranchEntity
                        {
                            Id = 371,
                            Name = "Ndola Branch",
                            BankId = "36",
                            BranchCode = "3609"
                        },
                        new BranchEntity
                        {
                            Id = 372,
                            Name = "Lumumba Branch",
                            BankId = "36",
                            BranchCode = "3610"
                        },
                        new BranchEntity
                        {
                            Id = 373,
                            Name = "Mazabuka Branch",
                            BankId = "36",
                            BranchCode = "3611"
                        },
                        new BranchEntity
                        {
                            Id = 374,
                            Name = "Head Office Branch",
                            BankId = "37",
                            BranchCode = "3701"
                        },
                        new BranchEntity
                        {
                            Id = 375,
                            Name = "Kamwala",
                            BankId = "37",
                            BranchCode = "3702"
                        },
                        new BranchEntity
                        {
                            Id = 376,
                            Name = "Cairo",
                            BankId = "37",
                            BranchCode = "3703"
                        },
                        new BranchEntity
                        {
                            Id = 377,
                            Name = "Kitwe",
                            BankId = "37",
                            BranchCode = "3704"
                        },
                        new BranchEntity
                        {
                            Id = 378,
                            Name = "Ndola",
                            BankId = "37",
                            BranchCode = "3705"
                        }




                    

            };

       


        }

        private static long _lastTimeStamp = 1;

        public static byte[] GetTimeStamp()
        {
            var stamp = System.Threading.Interlocked.Add(ref _lastTimeStamp, 1);
            return System.Text.ASCIIEncoding.ASCII.GetBytes(stamp.ToString());
        }

    }
}
