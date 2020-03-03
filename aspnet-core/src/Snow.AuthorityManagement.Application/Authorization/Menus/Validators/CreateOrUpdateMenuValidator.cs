using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;

namespace Snow.AuthorityManagement.Application.Authorization.Menus.Validators
{
    public class CreateOrUpdateMenuValidator : TemplateValidator<CreateOrUpdateMenu>
    {
        public CreateOrUpdateMenuValidator()
        {
            RuleFor(m => m.Menu)
                .NotNull()
                .WithMessage("{PropertyName}是必须的。")
                .NotEmpty()
                .WithMessage("{PropertyName}是必须的。")
                .SetValidator(new MenuEditValidator());
        }
    }
}