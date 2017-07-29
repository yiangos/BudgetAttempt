using AutoMapper;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using BFI = BudgetAttempt.Finance.Interfaces;
using BFR = BudgetAttempt.Finance.Repositories;
using BDM = BudgetAttempt.Data.Models;
using BDI = BudgetAttempt.Data.Interfaces;
using BDS = BudgetAttempt.Data.Storage;
using BFM = BudgetAttempt.Finance.Models;
using BFH = BudgetAttempt.Finance.Helpers;
using System.Configuration;

namespace BudgetAttempt.API.IoC
{
    public class NinjectInstanceProvider : IInstanceProvider
    {
        private Type serviceType;
        private IKernel kernel;

        public NinjectInstanceProvider(IKernel kernel, Type serviceType)
        {
            this.kernel = kernel;
            this.serviceType = serviceType;
            this.kernel.Bind<BFI.IFinanceRepository>().To<BFR.FinanceEntryRepository>();
            this.kernel.Bind<BFI.IPersonRepository>().To<BFR.PersonRepository>();
            this.kernel.Bind<BDI.IBudgetStorage>().To<BDS.SQLite.SQLiteBudgetStorage>().WithConstructorArgument("ConnectionString", ConfigurationManager.ConnectionStrings["BudgetDbDsn"].ConnectionString);
            this.kernel.Bind<Resolvers.IFinancePersonResolver>().To<Resolvers.FinancePersonResolver>();
            this.kernel.Bind<Resolvers.IFinanceCategoryResolver>().To<Resolvers.FinanceCategoryResolver>();
            Mapper.Initialize(cfg =>
            {
                cfg.ConstructServicesUsing(type => kernel.Get(type));
                cfg.CreateMap<BDM.Person, BFM.Person>();
                cfg.CreateMap<BFM.Person, Models.Person>();
                cfg.CreateMap<Models.Person, BFM.Person>();
                cfg.CreateMap<BFM.Person, BDM.Person>();

                cfg.CreateMap<BDM.Category, BFM.FinanceCategory>()
                                    .ForMember(dest => dest.FinanceType, opt => opt.MapFrom(src => (BFM.FinanceType)src.BudgetType));
                cfg.CreateMap<BFM.FinanceCategory, Models.TransactionCategory>()
                                    .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.FinanceType.ToString()));
                cfg.CreateMap<Models.TransactionCategory, BFM.FinanceCategory>()
                                    .ForMember(dest => dest.FinanceType, opt => opt.MapFrom(src => (BFM.FinanceType)Enum.Parse(typeof(BFM.FinanceType), src.TransactionType)));
                cfg.CreateMap<BFM.FinanceCategory, BDM.Category>()
                                    .ForMember(dest => dest.BudgetType, opt => opt.MapFrom(src => (int)(src.FinanceType)));

                cfg.CreateMap<BDM.BudgetLine, BFM.FinanceEntry>()
                                    .ForMember(dest => dest.FinanceType, opt => opt.MapFrom(src => (BFM.FinanceType)src.BudgetType))
                                    .ForMember(dest => dest.Person, opt => opt.ResolveUsing<Resolvers.FinancePersonResolver>())
                                    .ForMember(dest => dest.Category, opt => opt.ResolveUsing<Resolvers.FinanceCategoryResolver>());
                cfg.CreateMap<BFM.FinanceEntry, Models.Transaction>()
                                    .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.FinanceType.ToString()))
                                    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)));
                cfg.CreateMap<Models.Transaction, BFM.FinanceEntry>()
                                    .ForMember(dest => dest.FinanceType, opt => opt.MapFrom(src => (BFM.FinanceType)Enum.Parse(typeof(BFM.FinanceType), src.TransactionType)))
                                    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.ParseExact(src.Date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None)));
                cfg.CreateMap<BFM.FinanceEntry, BDM.BudgetLine>()
                                    .ForMember(dest => dest.BudgetType, opt => opt.MapFrom(src => (int)(src.FinanceType)))
                                    .ForMember(dest => dest.person, opt => opt.MapFrom(src => src.Person.Id))
                                    .ForMember(dest => dest.category, opt => opt.MapFrom(src => src.Category.Id));
            });
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return this.GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            // Create the instance with your IoC container of choice, here I use Ninject.
            return kernel.Get(this.serviceType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            kernel.Release(instance);
        }
    }
}