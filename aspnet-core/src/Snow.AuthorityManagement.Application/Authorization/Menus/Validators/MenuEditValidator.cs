using FluentValidation;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using Snow.AuthorityManagement.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Menus.Validators
{
    /// <summary>
    /// MenuEditDto验证
    /// </summary>
    public class MenuEditValidator : TemplateValidator<MenuEditDto>
    {
        /// <summary>
        /// 构造
        /// </summary>
        public MenuEditValidator()
        {
            RuleFor(u => u.Name)
                .NotNull()
                .WithMessage("{PropertyName}是必须的。")
                .NotEmpty()
                .WithMessage("{PropertyName}是必须的。")
                .MaximumLength(AuthorityManagementConsts.MaxNameLength)
                .WithMessage("{PropertyName}长度不能超过{MaxLength}。");
            RuleFor(u => u.Sort)
                .GreaterThanOrEqualTo(0)
                .WithMessage("{PropertyName}最小值为0");
        }
    }
}