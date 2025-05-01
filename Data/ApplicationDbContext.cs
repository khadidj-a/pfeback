using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tables PFE_PROJECT
        public DbSet<Marque> Marques { get; set; } = null!;
        public DbSet<Equipement> Equipements { get; set; } = null!;
        public DbSet<TypeEquip> TypeEquips { get; set; } = null!;
        public DbSet<Categorie> Categories { get; set; } = null!;
        public DbSet<Unite> Unites { get; set; } = null!;
        public DbSet<Wilaya> Wilayas { get; set; } = null!;
        public DbSet<Region> Regions { get; set; } = null!;
        public DbSet<Affectation> Affectations { get; set; } = null!;
        public DbSet<Caracteristique> Caracteristiques { get; set; } = null!;
        public DbSet<CaracteristiqueEquipement> CaracteristiqueEquipements { get; set; } = null!;
        public DbSet<Organe> Organes { get; set; } = null!;
        public DbSet<OrganeEquipement> OrganeEquipements { get; set; } = null!;
        public DbSet<GroupeIdentique> GroupeIdentiques { get; set; } = null!;
        public DbSet<GroupeOrgane> GroupeOrganes { get; set; } = null!;
        public DbSet<GroupeCaracteristique> GroupeCaracteristiques { get; set; } = null!;
        public DbSet<OrganeCaracteristique> OrganeCaracteristiques { get; set; } = null!;
        public DbSet<Reforme> Reformes { get; set; } = null!;
        public DbSet<Reaffectation> Reaffectations { get; set; } = null!;
        public DbSet<Pret> Prets { get; set; } = null!;

        // Tables UserManagement
        public DbSet<Utilisateur> Utilisateurs { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // PFE_PROJECT Configurations
            modelBuilder.Entity<Marque>().ToTable("marque");
            modelBuilder.Entity<Equipement>().ToTable("equipement");
            modelBuilder.Entity<TypeEquip>().ToTable("typequip");
            modelBuilder.Entity<Categorie>().ToTable("categorie");
            modelBuilder.Entity<Affectation>().ToTable("affectation");
            modelBuilder.Entity<Unite>().ToTable("unite");
            modelBuilder.Entity<Region>().ToTable("region");
            modelBuilder.Entity<Wilaya>().ToTable("wilaya");
            modelBuilder.Entity<Caracteristique>().ToTable("caracteristique");
            
            modelBuilder.Entity<CaracteristiqueEquipement>()
                .ToTable("caracteristiquequipement")
                .HasKey(ce => new { ce.idcarac, ce.ideqpt });

            modelBuilder.Entity<Organe>().ToTable("organe");

            modelBuilder.Entity<OrganeEquipement>()
                .ToTable("organequipement")
                .HasKey(oe => new { oe.idorg, oe.ideqpt });

            modelBuilder.Entity<GroupeIdentique>().ToTable("groupe_identique");

            modelBuilder.Entity<GroupeOrgane>()
                .ToTable("groupeorgane")
                .HasKey(go => new { go.idorg, go.idgrpidq });

            modelBuilder.Entity<GroupeOrgane>()
                .HasOne(go => go.Organe)
                .WithMany()
                .HasForeignKey(go => go.idorg);

            modelBuilder.Entity<GroupeOrgane>()
                .HasOne(go => go.GroupeIdentique)
                .WithMany(g => g.GroupeOrganes)
                .HasForeignKey(go => go.idgrpidq);

            modelBuilder.Entity<GroupeCaracteristique>()
                .ToTable("groupecaracteristique")
                .HasKey(gc => new { gc.idcarac, gc.idgrpidq });

            modelBuilder.Entity<GroupeCaracteristique>()
                .HasOne(gc => gc.Caracteristique)
                .WithMany()
                .HasForeignKey(gc => gc.idcarac);

            modelBuilder.Entity<GroupeCaracteristique>()
                .HasOne(gc => gc.GroupeIdentique)
                .WithMany(g => g.GroupeCaracteristiques)
                .HasForeignKey(gc => gc.idgrpidq);

            modelBuilder.Entity<OrganeCaracteristique>()
                .ToTable("organe_caracteristique")
                .HasKey(oc => new { oc.id_organe, oc.id_caracteristique });

            modelBuilder.Entity<OrganeCaracteristique>()
                .HasOne(oc => oc.Organe)
                .WithMany(o => o.OrganeCaracteristiques)
                .HasForeignKey(oc => oc.id_organe);

            modelBuilder.Entity<OrganeCaracteristique>()
                .HasOne(oc => oc.Caracteristique)
                .WithMany()
                .HasForeignKey(oc => oc.id_caracteristique);

            modelBuilder.Entity<Reforme>().ToTable("reforme");
            modelBuilder.Entity<Reaffectation>().ToTable("reaffectation");
            modelBuilder.Entity<Pret>().ToTable("pret");

            // UserManagement Configurations
            modelBuilder.Entity<Utilisateur>()
                .ToTable("utilisateur")
                .HasIndex(u => u.email)
                .IsUnique();

            modelBuilder.Entity<Utilisateur>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Utilisateurs)
                .HasForeignKey(u => u.idrole);

            modelBuilder.Entity<Role>().ToTable("role");
        }
    }
}
