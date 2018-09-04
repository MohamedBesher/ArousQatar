using Autofac;
using Autofac.Integration.WebApi;
using Saned.ArousQatar.Data.Core;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using Saned.ArousQatar.Data.Persistence.Repositories;
using System.Reflection;
using System.Web.Http;
using Saned.ArousQatar.Data.Core.Models;

namespace Saned.ArousQatar.Api
{
    public class AutofacWebapiConfig
    {
        public static IContainer Container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<ApplicationDbContext>()
             .As<IApplicationDbContext>()
             .InstancePerRequest();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();

            //builder.RegisterType<ApplicationDbContext>()
            //    .As<IdentityDbContext<ApplicationUser>>()
            //    .InstancePerRequest();
            //
            builder.RegisterGeneric(typeof(ApplicationUserStore<>))
                .As(typeof(IUserCustomStore<>))
                .InstancePerRequest();

            builder.RegisterType<ApplicationUserStoreImpl>()
                .As<IUserCustomStore<ApplicationUser>>()
                .InstancePerRequest();




            builder.RegisterType<DbFactory>()
                .As<IDbFactory>()
                .InstancePerRequest();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();

            builder.RegisterGeneric(typeof(EntityBaseRepository<>))
                .As(typeof(IEntityBaseRepository<>))
                .InstancePerRequest();

            builder.RegisterType<AdvertisementImageRepository>()
                            .As<IAdvertisementImageRepository>()
                            .InstancePerRequest();


            builder.RegisterType<AdvertisementRepository>()
                            .As<IAdvertisementRepository>()
                            .InstancePerRequest();

            builder.RegisterType<AdvertismentPriceRepository>()
                            .As<IAdvertisementPriceRepository>()
                            .InstancePerRequest();

            builder.RegisterType<BankAccountRepository>()
                .As<IBankAccountRepository>()
                .InstancePerRequest();


            builder.RegisterType<CategoryRepository>()
                .As<ICategoryRepository>()
                .InstancePerRequest();


            builder.RegisterType<CommentRepository>()
                .As<ICommentRepository>()
                .InstancePerRequest();


            builder.RegisterType<ComplaintRepository>()
                .As<IComplaintRepository>()
                .InstancePerRequest();


            builder.RegisterType<ContactInformationRepository>()
                .As<IContactInformationRepository>()
                .InstancePerRequest();


            builder.RegisterType<ContactTypeRepository>()
                .As<IContactTypeRepository>()
                .InstancePerRequest();


            builder.RegisterType<ErrorRepository>()
                .As<IErrorRepository>()
                .InstancePerRequest();


            builder.RegisterType<FavoriteRepository>()
                .As<IFavoriteRepository>()
                .InstancePerRequest();


            builder.RegisterType<LikeRepository>()
                .As<ILikeRepository>()
                .InstancePerRequest();
            //
            builder.RegisterType<EmailSettingRepository>()
               .As<IEmailSettingRepository>()
               .InstancePerRequest();

            //builder.RegisterType<AuthRepository>()
            //   .As<IAuthRepository>()
            //   .InstancePerRequest();

            //
            //builder.RegisterType<ApplicationUser>()
            //               .As<IdentityUser>()
            //               .InstancePerRequest();


            Container = builder.Build();

            return Container;
        }
    }
}