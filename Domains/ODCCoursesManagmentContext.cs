using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Domains
{
    public partial class ODCCoursesManagmentContext : DbContext
    {
        public ODCCoursesManagmentContext()
        {
        }

        public ODCCoursesManagmentContext(DbContextOptions<ODCCoursesManagmentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbAdmin> TbAdmins { get; set; }
        public virtual DbSet<TbCategroie> TbCategroies { get; set; }
        public virtual DbSet<TbCourse> TbCourses { get; set; }
        public virtual DbSet<TbEnroll> TbEnrolls { get; set; }
        public virtual DbSet<TbExam> TbExams { get; set; }
        public virtual DbSet<TbQuestion> TbQuestions { get; set; }
        public virtual DbSet<TbRevision> TbRevisions { get; set; }
        public virtual DbSet<TbStudent> TbStudents { get; set; }
        public virtual DbSet<TbTeaching> TbTeachings { get; set; }
        public virtual DbSet<TbTrainer> TbTrainers { get; set; }
        public virtual DbSet<TbVerificationCode> TbVerificationCodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-62P7HA1\\SQLEXPRESS;Database=ODC Courses Managment;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TbAdmin>(entity =>
            {
                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.Username).IsRequired();
            });

            modelBuilder.Entity<TbCategroie>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryName).IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TbCourse>(entity =>
            {
                entity.HasKey(e => e.CourseId);

                entity.HasIndex(e => e.CategoryId, "IX_TbCourses_CategoryId");

                entity.Property(e => e.CourseLevel).IsRequired();

                entity.Property(e => e.CourseName).IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TbCourses)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_TbCourses_TbCategroies");
            });

            modelBuilder.Entity<TbEnroll>(entity =>
            {
                entity.ToTable("TbEnroll");

                entity.HasIndex(e => e.CourseId, "IX_TbEnroll_CourseId");

                entity.HasIndex(e => e.StudentId, "IX_TbEnroll_StudentId");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.TbEnrolls)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_TbEnroll_TbCourses");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TbEnrolls)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_TbEnroll_TbStudents");
            });

            modelBuilder.Entity<TbExam>(entity =>
            {
                entity.HasKey(e => e.ExamId);

                entity.HasIndex(e => e.CourseId, "IX_TbExams_CourseId");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.TbExams)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_TbExams_TbCourses");
            });

            modelBuilder.Entity<TbQuestion>(entity =>
            {
                entity.HasIndex(e => e.ExamId, "IX_TbQuestions_ExamId");

                entity.Property(e => e.FirstChoice).IsRequired();

                entity.Property(e => e.FourthChoice).IsRequired();

                entity.Property(e => e.QuestionContent).IsRequired();

                entity.Property(e => e.QuestionRightAnswer).IsRequired();

                entity.Property(e => e.SecondChoice).IsRequired();

                entity.Property(e => e.ThirdChoice).IsRequired();

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.TbQuestions)
                    .HasForeignKey(d => d.ExamId)
                    .HasConstraintName("FK_TbQuestions_TbExams");
            });

            modelBuilder.Entity<TbRevision>(entity =>
            {
                entity.ToTable("TbRevision");

                entity.HasIndex(e => e.ExamId, "IX_TbRevision_ExamId");

                entity.HasIndex(e => e.StudentId, "IX_TbRevision_StudentId");

                entity.Property(e => e.StudentDegree).HasColumnType("decimal(4, 2)");

                entity.Property(e => e.TotalRightDegrees).HasColumnType("decimal(4, 2)");

                entity.Property(e => e.TotalWrongDegrees).HasColumnType("decimal(4, 2)");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.TbRevisions)
                    .HasForeignKey(d => d.ExamId)
                    .HasConstraintName("FK_TbRevision_TbExams");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TbRevisions)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_TbRevision_TbStudents");
            });

            modelBuilder.Entity<TbStudent>(entity =>
            {
                entity.HasKey(e => e.StudentId);
            });

            modelBuilder.Entity<TbTeaching>(entity =>
            {
                entity.ToTable("TbTeaching");

                entity.HasIndex(e => e.CourseId, "IX_TbTeaching_CourseId");

                entity.HasIndex(e => e.TrainerId, "IX_TbTeaching_TrainerId");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.TbTeachings)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_TbTeaching_TbCourses");

                entity.HasOne(d => d.Trainer)
                    .WithMany(p => p.TbTeachings)
                    .HasForeignKey(d => d.TrainerId)
                    .HasConstraintName("FK_TbTeaching_TbTrainers");
            });

            modelBuilder.Entity<TbTrainer>(entity =>
            {
                entity.HasKey(e => e.TrainerId);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<TbVerificationCode>(entity =>
            {
                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.TbVerificationCodes)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_TbVerificationCodes_TbCourses");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
