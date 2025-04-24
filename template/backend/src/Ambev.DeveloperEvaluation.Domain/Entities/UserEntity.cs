using System;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class UserEntity
    {
        [Key]
        public Guid Id { get; set; }   

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }   

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }   

        [Required]
        public string PasswordHash { get; set; }   

        [Required]
        public string FullName { get; set; }   
        public bool IsActive { get; set; }  

        public DateTime CreatedAt { get; set; }   

        public DateTime? UpdatedAt { get; set; }   

        public UserEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
    }
}
