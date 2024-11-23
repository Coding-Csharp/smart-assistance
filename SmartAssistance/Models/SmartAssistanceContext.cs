using Microsoft.EntityFrameworkCore;

namespace SmartAssistance.Models
{
    public partial class SmartAssistanceContext : DbContext
    {
        public SmartAssistanceContext
            (DbContextOptions<SmartAssistanceContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_area_id");

                entity.ToTable("areas");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Assign>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_assign_id");

                entity.ToTable("assigns");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.EmployeesId)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("employees_id");
                entity.Property(e => e.PositionsId).HasColumnName("positions_id");

                entity.HasOne(d => d.Employee).WithMany(p => p.Assigns)
                    .HasForeignKey(d => d.EmployeesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_assigns_employees_id");

                entity.HasOne(d => d.Position).WithMany(p => p.Assigns)
                    .HasForeignKey(d => d.PositionsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_assigns_positions_id");
            });

            modelBuilder.Entity<Assist>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_assist_id");

                entity.ToTable("assists");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CheckIn)
                    .HasColumnType("datetime")
                    .HasColumnName("check_in");
                entity.Property(e => e.CheckOut)
                    .HasColumnType("datetime")
                    .HasColumnName("check_out");
                entity.Property(e => e.EmotionalState)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("emotional_state");
                entity.Property(e => e.EmployeesId)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("employees_id");
                entity.Property(e => e.MinuteLate).HasColumnName("minute_late");

                entity.HasOne(d => d.Employee).WithMany(p => p.Assists)
                    .HasForeignKey(d => d.EmployeesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_assists_employees_id");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_employee_id");

                entity.ToTable("employees");

                entity.Property(e => e.Id)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("id");
                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("address");
                entity.Property(e => e.Birthdate).HasColumnName("birthdate");
                entity.Property(e => e.DateEntry).HasColumnName("date_entry");
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");
                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstname");
                entity.Property(e => e.Genre)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("genre");
                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastname");
                entity.Property(e => e.Nationality)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nationality");
                entity.Property(e => e.Phone).HasColumnName("phone");
                entity.Property(e => e.RolesId).HasColumnName("roles_id");
                entity.Property(e => e.SpecialtiesId).HasColumnName("specialties_id");
                entity.Property(e => e.State)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("state");
                entity.Property(e => e.TypeDocument)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("type_document");
                entity.Property(e => e.UrlImage)
                    .IsUnicode(false)
                    .HasColumnName("url_image");
                entity.Property(e => e.ZoneAccess)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("zone_access");

                entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                    .HasForeignKey(d => d.RolesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_employees_roles_id");

                entity.HasOne(d => d.Specialty).WithMany(p => p.Employees)
                    .HasForeignKey(d => d.SpecialtiesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_employees_specialties_id");
            });

            modelBuilder.Entity<EmployeeCredential>(entity =>
            {
                entity.HasKey(e => e.EmployeesId).HasName("pk_employee_credential_employees_id");

                entity.ToTable("employees_credentials");

                entity.Property(e => e.EmployeesId)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("employees_id");
                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.HasOne(d => d.Employee).WithOne(p => p.EmployeeCredential)
                    .HasForeignKey<EmployeeCredential>(d => d.EmployeesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_employees_credentials_employees_id");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_position_id");

                entity.ToTable("positions");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AreasId).HasColumnName("areas_id");
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.HasOne(d => d.Area).WithMany(p => p.Positions)
                    .HasForeignKey(d => d.AreasId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_positions_areas_id");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_role_id");

                entity.ToTable("roles");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Specialty>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_specialty_id");

                entity.ToTable("specialties");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}