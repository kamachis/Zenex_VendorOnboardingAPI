


using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Zenex.Authentication.Models.AuthMaster;
using static Zenex.Registration.Models.Registeration;
using System.Data;
using static Zenex.Master.Models.Master;

namespace Zenex.DBContext
{
    public class ZenexContext : DbContext
    {
        public ZenexContext(DbContextOptions options) : base(options)
        {
        }
        public ZenexContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleAppMap>(
            build =>
            {
                build.HasKey(t => new { t.RoleID, t.AppID });
                //build.HasOne(t => t.RoleID).WithOne().HasForeignKey<Role>(qe => qe.RoleID);
                //build.HasOne(t => t.AppID).WithOne().HasForeignKey<App>(qe => qe.AppID);
            });
            modelBuilder.Entity<UserRoleMap>(
            build =>
            {
                build.HasKey(t => new { t.UserID, t.RoleID });
                //build.HasOne(t => t.RoleID).WithOne().HasForeignKey<Role>(qe => qe.RoleID);
                //build.HasOne(t => t.AppID).WithOne().HasForeignKey<App>(qe => qe.AppID);
            });
            modelBuilder.Entity<UserPlantMap>().HasKey(table => new { table.UserID, table.Plant });
            //modelBuilder.Entity<Registration.Models.Registeration.BPIdentity>().HasKey(table => new { table.TransID, table.Type });
            modelBuilder.Entity<Registration.Models.Registeration.BPBank>().HasKey(table => new { table.TransID, table.AccountNo });
            modelBuilder.Entity<Registration.Models.Registeration.BPContact>().HasKey(table => new { table.TransID, table.Name });
            //modelBuilder.Entity<BPActivityLog>().HasKey(table => new { table.TransID, table.LogID });


            modelBuilder.Entity<CBPType>().HasKey(table => new {
                table.Type,
                table.Language
            });
            modelBuilder.Entity<CBPTitle>().HasKey(table => new {
                table.Title,
                table.Language
            });
            modelBuilder.Entity<CBPDepartment>().HasKey(table => new {
                table.Department,
                table.Language
            });
            modelBuilder.Entity<CBPFieldMaster>().HasKey(table => new { table.ID });
            modelBuilder.Entity<CBPFieldMaster>().HasIndex(table => new { table.Field });
        }


        

        public DbSet<Registration.Models.Registeration.BPVendorOnBoarding> BPVendorOnBoardings { get; set; }
        //public DbSet<Registration.Models.Registeration.BPIdentity> BPIdentities { get; set; }
        public DbSet<Registration.Models.Registeration.BPBank> BPBanks { get; set; }

        // public DbSet<BPIdentity> BPIdentity { get; set; }

        public DbSet<Registration.Models.Registeration.BPContact> BPContacts { get; set; }
        //public DbSet<BPActivityLog> BPActivityLogs { get; set; }
        public DbSet<BPText> BPTexts { get; set; }
        public DbSet<IdentityAttachments> IdentityAttachments { get; set; }
       // public DbSet<BPAttachment> BPAttachments { get; set; }
        public DbSet<Authentication.Models.AuthMaster.TokenHistory> TokenHistories { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<BPIdentity>().HasKey(table => new { table.TransID, table.Type });
        //    modelBuilder.Entity<BPBank>().HasKey(table => new { table.TransID, table.AccountNo });
        //    modelBuilder.Entity<BPContact>().HasKey(table => new { table.TransID, table.Item });
        //    modelBuilder.Entity<BPActivityLog>().HasKey(table => new { table.TransID, table.LogID });
        //}

        public DbSet<Client> Clients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<GSTField> GSTFields { get; set; }
        public DbSet<UserRoleMap> UserRoleMaps { get; set; }
        public DbSet<App> Apps { get; set; }
        public DbSet<RoleAppMap> RoleAppMaps { get; set; }
        public DbSet<UserLoginHistory> UserLoginHistory { get; set; }
        public DbSet<SessionMaster> SessionMasters { get; set; }
        public DbSet<UserPlantMap> UserPlantMaps { get; set; }
        public DbSet<VendorDetails> VendorDetails { get; set; }
       public DbSet<VendorRegisterLogin> VendorRegisterLogin { get; set; }
        public DbSet<Registration.Models.Registeration.TokenHistory> RegisterTokenHistories { get; set; }



        public DbSet<CBPType> CBPTypes { get; set; }
        public DbSet<CBPPostal> CBPPostals { get; set; }
        public DbSet<CBPIdentity> CBPIdentities { get; set; }
        public DbSet<CBPBank> CBPBanks { get; set; }
        public DbSet<CBPTitle> CBPTitles { get; set; }
        public DbSet<CBPDepartment> CBPDepartments { get; set; }
        public DbSet<CBPApp> CBPApps { get; set; }
        public DbSet<CBPLocation> CBPLocations { get; set; }
        public DbSet<CBPGstin> CBPGstins { get; set; }
        public DbSet<CBPFieldMaster> CBPFieldMasters { get; set; }
    }


  

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<CBPType>().HasKey(table => new {
    //        table.Type,
    //        table.Language
    //    });
    //    modelBuilder.Entity<CBPTitle>().HasKey(table => new {
    //        table.Title,
    //        table.Language
    //    });
    //    modelBuilder.Entity<CBPDepartment>().HasKey(table => new {
    //        table.Department,
    //        table.Language
    //    });
    //    modelBuilder.Entity<CBPFieldMaster>().HasKey(table => new { table.ID });
    //    modelBuilder.Entity<CBPFieldMaster>().HasIndex(table => new { table.Field });
    //}
    public class Role
    {
        [Key]
        public Guid RoleID { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class UserRoleMap
    {
        [Column(Order = 0), Key, ForeignKey("User")]
        public Guid UserID { get; set; }
        [Column(Order = 1), Key, ForeignKey("Role")]
        public Guid RoleID { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
   
}

