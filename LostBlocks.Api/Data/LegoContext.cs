using LostBlocks.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LostBlocks.Api.Data;

public class LegoContext(DbContextOptions<LegoContext> options) : DbContext(options)
{
    public virtual DbSet<LegoColor> Colors { get; set; }

    public virtual DbSet<LegoInventory> Inventories { get; set; }

    public virtual DbSet<LegoInventoryPart> InventoryParts { get; set; }

    public virtual DbSet<LegoInventorySet> InventorySets { get; set; }

    public virtual DbSet<LegoPart> Parts { get; set; }

    public virtual DbSet<LegoPartCategory> PartCategories { get; set; }

    public virtual DbSet<LegoSet> Sets { get; set; }

    public virtual DbSet<LegoTheme> Themes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO Scaffolding did not generate proper required columns, add IsRequired when needed.
        // TODO Database is missing proper foreign key, add foreign keys when needed.
        // TODO Cascade delete rules

        modelBuilder.UseSerialColumns();

        // EF Core defaults to DeleteBehaviour.Cascade which can lead to accidental deletion of object graphs.
        foreach (IMutableForeignKey mutableForeignKey in modelBuilder
                     .Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            mutableForeignKey.DeleteBehavior = DeleteBehavior.NoAction;
        }

        modelBuilder.Entity<LegoColor>(entity =>
        {
            entity
                .HasKey(e => e.Id)
                .HasName("lego_colors_pkey");

            entity.ToTable("lego_colors");

            entity
                .Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('lego_colors_id_seq')")
                .IsRequired();

            entity
                .Property(e => e.IsTransparent)
                .HasMaxLength(1)
                .HasColumnName("is_trans")
                .HasConversion<CharToBoolConverter>()
                .IsRequired();

            entity
                .Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name")
                .IsRequired();

            entity
                .Property(e => e.Rgb)
                .HasMaxLength(6)
                .HasColumnName("rgb")
                .IsRequired();

            entity
                .HasMany<LegoInventoryPart>(e => e.InventoryParts)
                .WithOne(e => e.Color)
                .HasForeignKey(e => e.ColorId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<LegoInventory>(entity =>
        {
            entity
                .HasKey(e => e.Id)
                .HasName("lego_inventories_pkey");

            entity.ToTable("lego_inventories");

            entity
                .Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('lego_inventories_id_seq')");

            entity
                .Property(e => e.SetNum)
                .HasMaxLength(255)
                .HasColumnName("set_num");

            entity
                .Property(e => e.Version)
                .HasColumnName("version");

            entity
                .HasOne<LegoSet>(e => e.Set)
                .WithMany(e => e.Inventories)
                .HasForeignKey(e => e.SetNum)
                .HasPrincipalKey(e => e.SetNum);

            entity
                .HasMany<LegoInventoryPart>(e => e.InventoryParts)
                .WithOne(e => e.Inventory)
                .HasForeignKey(e => e.InventoryId)
                .HasPrincipalKey(e => e.Id);

            entity
                .HasMany<LegoInventorySet>(e => e.InventorySets)
                .WithOne(e => e.Inventory)
                .HasForeignKey(e => e.InventoryId)
                .HasPrincipalKey(e => e.Id);
        });

        modelBuilder.Entity<LegoInventoryPart>(entity =>
        {
            entity.ToTable("lego_inventory_parts");

            entity.HasKey(e => new { e.InventoryId, e.ColorId, e.PartNum, e.IsSpare });

            entity
                .Property(e => e.ColorId)
                .HasColumnName("color_id");

            entity
                .Property(e => e.InventoryId)
                .HasColumnName("inventory_id");

            entity
                .Property(e => e.IsSpare)
                .HasColumnName("is_spare");

            entity
                .Property(e => e.PartNum)
                .HasMaxLength(255)
                .HasColumnName("part_num");

            entity
                .Property(e => e.Quantity)
                .HasColumnName("quantity");

            entity
                .HasOne<LegoInventory>(e => e.Inventory)
                .WithMany(e => e.InventoryParts)
                .HasForeignKey(e => e.InventoryId)
                .HasPrincipalKey(e => e.Id);

            entity
                .HasOne<LegoColor>(e => e.Color)
                .WithMany(e => e.InventoryParts)
                .HasForeignKey(e => e.ColorId)
                .HasPrincipalKey(e => e.Id);

            entity
                .HasOne<LegoPart>(e => e.Part)
                .WithMany(p => p.InventoryParts)
                .HasForeignKey(e => e.PartNum)
                .HasPrincipalKey(e => e.PartNum);
        });

        modelBuilder.Entity<LegoInventorySet>(entity =>
        {
            entity.ToTable("lego_inventory_sets");

            entity
                .HasKey(e => new { e.InventoryId, e.SetNum });

            entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity
                .Property(e => e.SetNum)
                .HasMaxLength(255)
                .HasColumnName("set_num");

            entity
                .HasOne<LegoInventory>(e => e.Inventory)
                .WithMany(e => e.InventorySets)
                .HasForeignKey(e => e.InventoryId)
                .HasPrincipalKey(e => e.Id);

            entity
                .HasOne<LegoSet>(e => e.Set)
                .WithMany(e => e.InventorySets)
                .HasForeignKey(e => e.SetNum)
                .HasPrincipalKey(e => e.SetNum);
        });

        modelBuilder.Entity<LegoPart>(entity =>
        {
            entity.HasKey(e => e.PartNum).HasName("lego_parts_pkey");

            entity.ToTable("lego_parts");

            entity
                .Property(e => e.PartNum)
                .HasMaxLength(255)
                .HasColumnName("part_num");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.CategoryId).HasColumnName("part_cat_id");

            entity
                .HasOne<LegoPartCategory>(e => e.Category)
                .WithMany(e => e.Parts)
                .HasForeignKey(e => e.CategoryId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.NoAction);

            entity
                .HasMany<LegoInventoryPart>(e => e.InventoryParts)
                .WithOne(p => p.Part)
                .HasForeignKey(e => e.PartNum)
                .HasPrincipalKey(e => e.PartNum)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<LegoPartCategory>(entity =>
        {
            entity
                .HasKey(e => e.Id)
                .HasName("lego_part_categories_pkey");

            entity.ToTable("lego_part_categories");

            entity
                .Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('lego_part_categories_id_seq')");

            entity
                .Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity
                .HasMany<LegoPart>(e => e.Parts)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<LegoSet>(entity =>
        {
            entity
                .HasKey(e => e.SetNum)
                .HasName("lego_sets_pkey");

            entity.ToTable("lego_sets");

            entity
                .Property(e => e.SetNum)
                .HasMaxLength(255)
                .HasColumnName("set_num");

            entity
                .Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity
                .Property(e => e.NumParts)
                .HasColumnName("num_parts");

            entity
                .Property(e => e.ThemeId)
                .HasColumnName("theme_id");

            entity
                .Property(e => e.Year)
                .HasColumnName("year");

            entity
                .HasOne<LegoTheme>(e => e.Theme)
                .WithMany(e => e.Sets)
                .HasForeignKey(e => e.ThemeId)
                .HasPrincipalKey(e => e.Id);

            entity
                .HasMany<LegoInventory>(e => e.Inventories)
                .WithOne(e => e.Set)
                .HasForeignKey(e => e.SetNum)
                .HasPrincipalKey(e => e.SetNum);

            entity
                .HasMany<LegoInventorySet>(e => e.InventorySets)
                .WithOne(e => e.Set)
                .HasForeignKey(e => e.SetNum)
                .HasPrincipalKey(e => e.SetNum);
        });

        modelBuilder.Entity<LegoTheme>(entity =>
        {
            entity
                .HasKey(e => e.Id)
                .HasName("lego_themes_pkey");

            entity.ToTable("lego_themes");

            entity
                .Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('lego_themes_id_seq')");

            entity
                .Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity
                .Property(e => e.ParentId)
                .HasColumnName("parent_id");

            entity
                .HasOne(e => e.Parent)
                .WithMany(e => e.Childs)
                .HasForeignKey(e => e.ParentId)
                .HasPrincipalKey(e => e.Id);
        });

        modelBuilder.HasSequence<int>("lego_colors_id_seq");
        modelBuilder.HasSequence<int>("lego_inventories_id_seq");
        modelBuilder.HasSequence<int>("lego_part_categories_id_seq");
        modelBuilder.HasSequence<int>("lego_themes_id_seq");
    }
}

file class CharToBoolConverter() : ValueConverter<bool, char>(b => b == true ? 't' : 'f', c => c == 't');
