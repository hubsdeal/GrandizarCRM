using SoftGrid.CRM;
using SoftGrid.Territory;
using SoftGrid.LookupData;
using Abp.IdentityServer4vNext;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoftGrid.Authorization.Delegation;
using SoftGrid.Authorization.Roles;
using SoftGrid.Authorization.Users;
using SoftGrid.Chat;
using SoftGrid.Editions;
using SoftGrid.Friendships;
using SoftGrid.MultiTenancy;
using SoftGrid.MultiTenancy.Accounting;
using SoftGrid.MultiTenancy.Payments;
using SoftGrid.Storage;

namespace SoftGrid.EntityFrameworkCore
{
    public class SoftGridDbContext : AbpZeroDbContext<Tenant, Role, User, SoftGridDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<Contact> Contacts { get; set; }

        public virtual DbSet<Hub> Hubs { get; set; }

        public virtual DbSet<HubType> HubTypes { get; set; }

        public virtual DbSet<MembershipType> MembershipTypes { get; set; }

        public virtual DbSet<ContractType> ContractTypes { get; set; }

        public virtual DbSet<DocumentType> DocumentTypes { get; set; }

        public virtual DbSet<SmsTemplate> SmsTemplates { get; set; }

        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }

        public virtual DbSet<ConnectChannel> ConnectChannels { get; set; }

        public virtual DbSet<ZipCode> ZipCodes { get; set; }

        public virtual DbSet<RatingLike> RatingLikes { get; set; }

        public virtual DbSet<MeasurementUnit> MeasurementUnits { get; set; }

        public virtual DbSet<MasterTag> MasterTags { get; set; }

        public virtual DbSet<MasterTagCategory> MasterTagCategories { get; set; }

        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<County> Counties { get; set; }

        public virtual DbSet<State> States { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Currency> Currencies { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

        public SoftGridDbContext(DbContextOptions<SoftGridDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contact>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Hub>(h =>
                       {
                           h.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<HubType>(h =>
                       {
                           h.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<MembershipType>(m =>
                       {
                           m.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ContractType>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DocumentType>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<SmsTemplate>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<EmailTemplate>(x =>
                       {
                           x.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ConnectChannel>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ZipCode>(z =>
                       {
                           z.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<RatingLike>(r =>
                       {
                           r.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<MeasurementUnit>(m =>
                       {
                           m.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<MasterTag>(m =>
                       {
                           m.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<MasterTagCategory>(m =>
                       {
                           m.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<City>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<County>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<State>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Country>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Currency>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<BinaryObject>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique()
                    .HasFilter("[IsDeleted] = 0");
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}