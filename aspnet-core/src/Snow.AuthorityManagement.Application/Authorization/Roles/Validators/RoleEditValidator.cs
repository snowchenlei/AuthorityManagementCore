using FluentValidation;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Roles.Validators
{
    public class RoleEditValidator : TemplateValidator<RoleEditDto>
    {
        public RoleEditValidator()
        {
            RuleFor(r => r.Name)
                .NotNull()
                .WithMessage("{PropertyName}是必须的。")
                .NotEmpty()
                .WithMessage("{PropertyName}是必须的。")
                .MaximumLength(AuthorityManagementConsts.MaxNameLength)
                .WithMessage("{PropertyName}长度不能超过{MaxLength}。");
            RuleFor(r => r.DisplayName)
                .NotNull()
                .WithMessage("{PropertyName}是必须的。")
                .NotEmpty()
                .WithMessage("{PropertyName}是必须的。")
                .MaximumLength(AuthorityManagementConsts.MaxUserNameLength)
                .WithMessage("{PropertyName}长度不能超过{MaxLength}。");
        }
    }
}