// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore; // Asegúrate de que esta línea esté presente

namespace BlazorQuizApp.Data
{
    // Modifica esta línea para implementar IDataProtectionKeyContext
    public class ApplicationDbContext : DbContext, IDataProtectionKeyContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        
        // ¡Añade esta línea para el DbSet de Data Protection Keys!
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } 

        // Agrega más DbSets según tus necesidades para usuarios, etc.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Puedes configurar tus modelos aquí, por ejemplo, datos de prueba
            modelBuilder.Entity<Question>().HasData(
                new Question { Id = 1, Text = "¿Cuál es la capital de Francia?" },
                new Question { Id = 2, Text = "¿Cuánto es 2 + 2?" }
            );
            modelBuilder.Entity<Answer>().HasData(
                new Answer { Id = 1, QuestionId = 1, Text = "París", IsCorrect = true },
                new Answer { Id = 2, QuestionId = 1, Text = "Londres", IsCorrect = false },
                new Answer { Id = 3, QuestionId = 2, Text = "3", IsCorrect = false },
                new Answer { Id = 4, QuestionId = 2, Text = "4", IsCorrect = true }
            );
        }
    }

    // Data/Question.cs
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public ICollection<Answer> Answers { get; set; } = new List<Answer>(); // Inicializar la colección para evitar nulls
    }

    // Data/Answer.cs
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public Question? Question { get; set; } // Puede ser nulo en algunos escenarios de EF
    }
}