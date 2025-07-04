﻿using SkillForge.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkillForge.Models.Database;

[SelectOption(LabelProperty = nameof(DisplayedName))]
public class AdminRole : BaseEntity
{
    [StringLength(32, MinimumLength = 1)]
    [Column(TypeName = "varchar")]
    [Code]
    public string Code { get; set; }

    [StringLength(32, MinimumLength = 1)]
    [Column(TypeName = "varchar")]
    public string DisplayedName { get; set; }

    public List<AdminUser>? Admins { get; set; }
}
