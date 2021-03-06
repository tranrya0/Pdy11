﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ContosoUniversity.Models;

namespace ContosoUniversity.DAL
{
    public class SchoolContext : DbContext
    {
        public SchoolContext() : base ("SchoolContext")
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<Person> People { get; set; } // base class student and instructors inherit from


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Fluent API in place of the data attributes
            // this configures the many to many join table
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors).WithMany(i => i.Courses)
                .Map(t => t.MapLeftKey("CourseID")
                .MapRightKey("InstructorID")
                .ToTable("CourseInstructor"));

            // easy to add stored procedures in EF code first
            // this tells EF to use sstored procedures for insert, update, and delete 
            // operations on the Department entity
            modelBuilder.Entity<Department>().MapToStoredProcedures();

            // v.10 - concurrency, can alternatively use the fluent api
            //modelBuilder.Entity<Department>()
            //    .Property(p => p.RowVersion).IsConcurrencyToken();
        }
    }
}
