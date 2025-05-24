using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SkillForge.Attributes;

namespace SkillForge.Models.Database;

[SelectOption]
public class User : BaseEntity
{
    [StringLength(16)]
    [Column(TypeName = "varchar")]
    [TableColumn]
    public string Name { get; set; }
    
    [StringLength(32)]
    [EmailAddress]
    [Column(TypeName = "varchar")]
    [TableColumn]
    [Display(Name = "E-mail")]
    public string Email { get; set; }

    [StringLength(64)]
    [Column(TypeName = "varchar")]
    [Display(Name = "Avatar")]
    public string? AvatarPath { get; set; }

    [StringLength(256)]
    [TableColumn]
    public string? Bio { get; set; }

    [StringLength(60)]
    [Column(TypeName = "varchar")]
    public string PasswordHash { get; set; }

    [MinLength(16)]
    [MaxLength(16)]
    public byte[] PasswordHashSalt { get; set; }

    [TableColumn]
    [Display(Name = "Date of birth")]
    public DateTime? DateOfBirth { get; set; }

    public List<Article>? Articles { get; set; }

    public List<FavoriteArticle>? FavouriteArticles { get; set; }
}
