using SkillForge.Areas.Admin.Models.Components.Grid;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Exceptions;
using SkillForge.Models.Database;
using SkillForge.Services;

namespace SkillForge.Areas.Admin.Services;

public class AdminUserService : CrudService<AdminUser, AdminUserVM>, IAdminUserService
{
    private readonly IImageService imageService;
    private readonly IAuthService authService;
    private readonly IAdminRoleService adminRoleService;

    public AdminUserService(
        IAdminUserRepository repository,
        IImageService imageService,
        IAuthService authService,
        IAdminRoleService adminRoleService)
        : base(repository)
    {
        this.imageService = imageService;
        this.authService = authService;
        this.adminRoleService = adminRoleService;
    }

    public override RowAction CustomizeEditRowAction(RowAction action)
    {
        return base.CustomizeEditRowAction(action)
            .ExclusiveToRoles("admin");
    }

    public override RowAction CustomizeDeleteRowAction(RowAction action)
    {
        return base.CustomizeDeleteRowAction(action)
            .ExclusiveToRoles("admin");
    }

    public override async Task<Table<AdminUser>> CreateListingTable(ListingModel<AdminUser> listingModel, PaginatedList<AdminUser> items)
    {
        return (await base.CreateListingTable(listingModel, items))
            .SetSelectableOptionsSource(nameof(AdminUser.Role), await adminRoleService.GetAll());
    }

    public override AdminUser ViewModelToEntity(AdminUserVM model)
    {
        return new AdminUser
        {
            Id = model.Id,
            AvatarPath = model.CurrentAvatarFilename,
            Email = model.Email,
            Name = model.Username,
            RoleId = model.RoleId,
        };
    }

    public override AdminUserVM EntityToViewModel(AdminUser entity)
    {
        return new AdminUserVM
        {
            Id = entity.Id,
            Username = entity.Name,
            Email = entity.Email,
            CurrentAvatarFilename = entity.AvatarPath,
            RoleId = entity.RoleId,
        };
    }

    public override Task<bool> Upsert(AdminUserVM model)
    {
        throw new InvalidOperationException($"Invalid method used.");
    }

    public async Task<bool> Upsert(AdminUserVM model, bool isAdministrator)
    {
        AdminUser? existing = await Get(model.Id);
        AdminUser entity;

        if (existing != null)
        {
            if (string.IsNullOrEmpty(model.CurrentPassword))
            {
                throw new ModelValidationException(nameof(AdminUserVM.CurrentPassword), "The current password is required.");
            }
            if (!authService.CompareHashes(model.CurrentPassword, existing.PasswordHash, existing.PasswordHashSalt))
            {
                throw new ModelValidationException(nameof(AdminUserVM.CurrentPassword), "Incorrect password.");
            }

            entity = existing;
            entity.Name = model.Username;
            entity.AvatarPath = model.CurrentAvatarFilename;
            entity.Email = model.Email;

            if (isAdministrator)
            {
                entity.RoleId = model.RoleId;
            }
        }
        else
        {
            entity = ViewModelToEntity(model);
        }

        if (!string.IsNullOrEmpty(model.Password))
        {
            byte[] passwordHashSalt = authService.GenerateSalt();
            string passwordHash = authService.Hash(model.Password, passwordHashSalt);

            entity.PasswordHash = passwordHash;
            entity.PasswordHashSalt = passwordHashSalt;
        }
        else if (model.Id == 0)
        {
            throw new ModelValidationException(nameof(AdminUserVM.Password), "Must enter a valid password.");
        }

        if (model.AvatarImage != null)
        {
            if (model.CurrentAvatarFilename != null)
            {
                imageService.RemoveImage("avatars", model.CurrentAvatarFilename);
            }

            entity.AvatarPath = await imageService.UploadImageAsync(model.AvatarImage, "avatars");
        }
        else if (model.RemoveAvatarImage && model.CurrentAvatarFilename != null)
        {
            imageService.RemoveImage("avatars", model.CurrentAvatarFilename);

            entity.AvatarPath = null;
        }

        return await UpsertEntity(entity);
    }
}
