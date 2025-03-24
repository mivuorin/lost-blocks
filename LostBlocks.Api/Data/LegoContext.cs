using LostBlocks.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LostBlocks.Api.Data;

public class LegoContext(DbContextOptions<LegoContext> options) : DbContext(options)
{
    public virtual DbSet<LegoColor> Colors { get; set; }

    public virtual DbSet<LegoInventory> Inventories { get; set; }

    public virtual DbSet<LegoInventoryPart> InventoryParts { get; set; }

    public virtual DbSet<LegoInventorySet> InventorySets { get; set; }

    public virtual DbSet<LegoPart> Parts { get; set; }

    public virtual DbSet<LegoPartCategory> PartCategories { get; set; }

    public virtual DbSet<LegoSet> LegoSets { get; set; }

    public virtual DbSet<LegoTheme> Themes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LegoColor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lego_colors_pkey");

            entity.ToTable("lego_colors");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsTrans)
                .HasMaxLength(1)
                .HasColumnName("is_trans");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Rgb)
                .HasMaxLength(6)
                .HasColumnName("rgb");
        });

        modelBuilder.Entity<LegoInventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lego_inventories_pkey");

            entity.ToTable("lego_inventories");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SetNum)
                .HasMaxLength(255)
                .HasColumnName("set_num");
            entity.Property(e => e.Version).HasColumnName("version");
        });

        modelBuilder.Entity<LegoInventoryPart>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("lego_inventory_parts");

            entity.Property(e => e.ColorId).HasColumnName("color_id");
            entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
            entity.Property(e => e.IsSpare).HasColumnName("is_spare");
            entity.Property(e => e.PartNum)
                .HasMaxLength(255)
                .HasColumnName("part_num");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
        });

        modelBuilder.Entity<LegoInventorySet>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("lego_inventory_sets");

            entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SetNum)
                .HasMaxLength(255)
                .HasColumnName("set_num");
        });

        modelBuilder.Entity<LegoPart>(entity =>
        {
            entity.HasKey(e => e.PartNum).HasName("lego_parts_pkey");

            entity.ToTable("lego_parts");

            entity.Property(e => e.PartNum)
                .HasMaxLength(255)
                .HasColumnName("part_num");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.PartCatId).HasColumnName("part_cat_id");
        });

        modelBuilder.Entity<LegoPartCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lego_part_categories_pkey");

            entity.ToTable("lego_part_categories");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<LegoSet>(entity =>
        {
            entity.HasKey(e => e.SetNum).HasName("lego_sets_pkey");

            entity.ToTable("lego_sets");

            entity.Property(e => e.SetNum)
                .HasMaxLength(255)
                .HasColumnName("set_num");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.NumParts).HasColumnName("num_parts");
            entity.Property(e => e.ThemeId).HasColumnName("theme_id");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        modelBuilder.Entity<LegoTheme>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lego_themes_pkey");

            entity.ToTable("lego_themes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
        });
    }
}
