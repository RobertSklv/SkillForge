using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SkillForge.Attributes;

namespace SkillForge.Models.Database;

public class AdminUser : BaseEntity
{
    [StringLength(16)]
    [Column(TypeName = "varchar")]
    [TableColumn]
    public string Name { get; set; }

    [StringLength(32)]
    [EmailAddress]
    [Column(TypeName = "varchar")]
    [TableColumn(Name = "E-mail")]
    public string Email { get; set; }

    [StringLength(64)]
    [Column(TypeName = "varchar")]
    public string? AvatarPath { get; set; }

    [StringLength(60)]
    [Column(TypeName = "varchar")]
    public string PasswordHash { get; set; }

    [MinLength(16)]
    [MaxLength(16)]
    public byte[] PasswordHashSalt { get; set; }

    [TableColumn]
    public AdminRole? Role { get; set; }

    public int RoleId { get; set; }
}
