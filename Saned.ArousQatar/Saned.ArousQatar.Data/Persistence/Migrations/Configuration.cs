using Microsoft.AspNet.Identity.EntityFramework;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Tools;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Persistence\Migrations";
        }

        protected override void Seed(ApplicationDbContext context)
        {


            #region MyRegion
            //string adminRoleId;
            //string userRoleId;



            //if (!context.Roles.Any())
            //{
            //    adminRoleId = context.Roles.Add(new IdentityRole("Administrator")).Id;
            //    userRoleId = context.Roles.Add(new IdentityRole("User")).Id;

            //}
            //else
            //{
            //    adminRoleId = context.Roles.First(c => c.Name == "Administrator").Id;
            //    userRoleId = context.Roles.First(c => c.Name == "User").Id;

            //}

            //context.SaveChanges();
            
            //if (!context.Users.Any())
            //{
            //    var administrator = context.Users.Add(new ApplicationUser() { Name = "administrator", UserName = "administrator", Email = "admin@somesite.com", EmailConfirmed = true });
            //    administrator.Roles.Add(new IdentityUserRole { RoleId = adminRoleId });

            //    var standardUser = context.Users.Add(new ApplicationUser() { Name = "User", UserName = "shereen", Email = "jon@somesite.com", EmailConfirmed = true });
            //    standardUser.Roles.Add(new IdentityUserRole { RoleId = userRoleId });



            //    context.SaveChanges();

            //    var store = new ApplicationUserStoreImpl();//new UserStore<ApplicationUser>();

            //    store.SetPasswordHashAsync(administrator, new ApplicationUserManagerImpl().PasswordHasher.HashPassword("administrator123"));
            //    store.SetPasswordHashAsync(standardUser, new ApplicationUserManagerImpl().PasswordHasher.HashPassword("user123"));

            //}

            //if (!context.Clients.Any())
            //{
            //    context.Clients.AddRange(BuildClientsList());
            //    context.SaveChanges();
            //}
            #endregion
            //create categoties....
            //context.Categories.AddOrUpdate(GenreateCategories());
            //context.SaveChanges();
            ////create AdvertisementPrice ...
            //context.AdvertismentPrices.AddOrUpdate(GenerateAdvertismentPrice());
            //context.SaveChanges();
            ////create Advertisements ...
            //string iduser = context.Users.First(x => x.UserName == "shereen").Id;
            //context.Advertisments.AddOrUpdate(GenerateAdvertisment(iduser));
            //context.SaveChanges();
            ////Generate BankAccounts...
            //context.BankAccounts.AddOrUpdate(GenerateBankAccounts());
            //context.SaveChanges();
        }


        private static List<Client> BuildClientsList()
        {

            List<Client> clientsList = new List<Client>
            {
                new Client
                { Id = "ngAuthApp",
                    Secret= Helper.GetHash("abc@123"),
                    Name="AngularJS front-end Application",
                    ApplicationType =ApplicationTypes.JavaScript,
                    Active = true,
                    RefreshTokenLifeTime = 7200,
                    AllowedOrigin = "http://localhost:32150/"
                },
                new Client
                { Id = "consoleApp",
                    Secret=Helper.GetHash("123@abc"),
                    Name="Console Application",
                    ApplicationType =ApplicationTypes.NativeConfidential,
                    Active = true,
                    RefreshTokenLifeTime = 14400,
                    AllowedOrigin = "*"
                }
            };

            return clientsList;
        }
        private Category[] GenreateCategories()
        {
            Category[] categories = new Category[]
            {
                new Category()
                {
                    Name = "فساتين",
                    IconName = "ionicons ion-android-laptop",
                    IsArchieved = false
                },
                new Category()
                {
                    Name = "عود وعطور",
                    IconName = "ionicons ion-android-star",
                    IsArchieved = false
                },
                new Category()
                {
                    Name = "شنط وأبواك",
                    IconName = "ionicons ion-android-phone-portrait",
                    IsArchieved = false
                },
                new Category()
                {
                    Name = "أحذية",
                    IconName = "ionicons ion-ios-person-outline",
                    IsArchieved = false
                },
            };

            return categories;
        }
        private AdvertismentPrice[] GenerateAdvertismentPrice()
        {
            AdvertismentPrice[] advertismentPrice = new AdvertismentPrice[]
            {
                new AdvertismentPrice()
                {
                    Period ="3" ,
                    Price  = 20,
                    IsArchieved = false
                },
                new AdvertismentPrice()
                {
                    Period = "5" ,
                    Price  = 100,
                    IsArchieved = true
                },
                new AdvertismentPrice()
                {
                    Period = "10" ,
                    Price  = 200,
                    IsArchieved = false
                },
                new AdvertismentPrice()
                {
                    Period = "1",
                    Price  = 5,
                    IsArchieved = true
                },
                new AdvertismentPrice()
                {
                    Period = "6" ,
                    Price  = 40,
                    IsArchieved = false
                }
            };
            return advertismentPrice;
        }
        private Advertisment[] GenerateAdvertisment(string userid)
        {
            Advertisment[] advertisments = new Advertisment[]
            {
                new Advertisment()
                {
                    IsArchieved  = false,
                    CategoryId =  2 ,
                    AdvertismentPriceId = null,
                    Name = "إعلان عن قطع ملابس",
                    ApplicationUserId = userid,
                    EndDate = new DateTime(2016 , 12 , 27),
                    IsPaided = false,
                    Description = "قطع ملابس مستعمله",
                    NumberOfLikes = 10,
                    NumberOfViews = 12,
                    PaidEdPrice = 0,
                    StartDate = new DateTime(2016 , 12 , 24)
                },
                new Advertisment()
                {
                    IsArchieved  = true,
                    CategoryId =  3 ,
                    AdvertismentPriceId = null,
                    Name = "إعلان عن قطع إلكترونيات",
                    ApplicationUserId = userid,
                    EndDate = new DateTime(2016 , 12 , 27),
                    IsPaided = true,
                    Description = "قطع إلكترونيات",
                    NumberOfLikes = 20,
                    NumberOfViews = 23,
                    PaidEdPrice = 55,
                    StartDate = new DateTime(2016 , 12 , 24)
                },
                new Advertisment()
                {
                    IsArchieved  = false,
                    CategoryId =  4 ,
                    AdvertismentPriceId = null,
                    Name = "إعلان عن هواتف",
                    ApplicationUserId = userid,
                    EndDate = new DateTime(2016 , 12 , 27),
                    IsPaided = false,
                    Description = "قطع هواتف",
                    NumberOfLikes = 10,
                    NumberOfViews = 12,
                    PaidEdPrice = 0,
                    StartDate = new DateTime(2016 , 12 , 24)
                },
                new Advertisment()
                {
                    IsArchieved  = false,
                    CategoryId =  1 ,
                    AdvertismentPriceId = null ,
                    Name = "إعلان عن احذية",
                    ApplicationUserId = userid,
                    EndDate = new DateTime(2016 , 12 , 27),
                    IsPaided = false,
                    Description = "قطع احذية",
                    NumberOfLikes = 10,
                    NumberOfViews = 12,
                    PaidEdPrice = 0,
                    StartDate = new DateTime(2016 , 12 , 24)
                },
            };
            return advertisments;
        }
        private BankAccount[] GenerateBankAccounts()
        {
            var bankAccount = new BankAccount[]
            {
                new BankAccount()
                {
                    BankName = "CIB",
                    BankNumber = "05486523642365"
                },
                new BankAccount()
                {
                    BankName = "ASB",
                    BankNumber = "16121561321612"
                },
                new BankAccount()
                {
                    BankName = "CIB",
                    BankNumber = "12316512142626"
                },
                new BankAccount()
                {
                    BankName = "QNB",
                    BankNumber = "15651564323532"
                },
                new BankAccount()
                {
                    BankName = "ENB",
                    BankNumber = "16565965487132"
                },
                new BankAccount()
                {
                    BankName = "CIB",
                    BankNumber = "16184621321546"
                },
            };
            return bankAccount;
        }
    }
}
