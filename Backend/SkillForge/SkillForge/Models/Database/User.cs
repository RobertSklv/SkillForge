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

    [TableColumn(Format = "dd.MM.yyyy")]
    [Display(Name = "Date of birth")]
    public DateTime? DateOfBirth { get; set; }

    public List<Article>? Articles { get; set; }

    public List<FavoriteArticle>? FavoriteArticles { get; set; }

    public List<UserFollow>? Followings { get; set; }

    public List<UserFollow>? Followers { get; set; }

    public List<TagFollow>? TagsFollowed { get; set; }

    public List<AccountSuspension>? Suspensions { get; set; }

    public int FollowersCount { get; set; }

    public int FollowingsCount { get; set; }

    public int TagFollowingsCount { get; set; }

    public int ArticlesCount { get; set; }

    public bool IsSuspended => (Suspensions ?? throw new Exception("Suspensions not loaded")).Any(s => s.IsActive);

    public AccountSuspension? ActiveSuspension => (Suspensions ?? throw new Exception("Suspensions not loaded"))
        .FirstOrDefault(s => s.IsActive);
}
