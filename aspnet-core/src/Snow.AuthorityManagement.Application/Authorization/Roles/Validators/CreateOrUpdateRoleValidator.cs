using FluentValidation;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Roles.Validators
{
    public class CreateOrUpdateRoleValidator : TemplateValidator<CreateOrUpdateRole>
    {
        public CreateOrUpdateRoleValidator()
        {
            RuleFor(u => u.Role)
                .NotNull()
                .WithMessage("{PropertyName}是必须的。")
                .NotEmpty()
                .WithMessage("{PropertyName}是必须的。")
                .SetValidator(new RoleEditValidator());
        }
    }
}