using FluentValidation;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Users.Validators
{
    public class CreateOrUpdateUserValidator : TemplateValidator<CreateOrUpdateUser>
    {
        public CreateOrUpdateUserValidator()
        {
            RuleFor(u => u.User)
                .NotNull()
                .WithMessage("{PropertyName}是必须的。")
                .NotEmpty()
                .WithMessage("{PropertyName}是必须的。")
                .SetValidator(new UserEditValidator());
        }
    }
}