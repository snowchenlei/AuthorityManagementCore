using FluentValidation;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Users.Validators
{
    public class UserEditValidator : TemplateValidator<UserEditDto>
    {
        public UserEditValidator()
        {
            RuleFor(u => u.Name)
                .NotNull()
                .WithMessage("{PropertyName}是必须的。")
                .NotEmpty()
                .WithMessage("{PropertyName}是必须的。")
                .MaximumLength(AuthorityManagementConsts.MaxNameLength)
                .WithMessage("{PropertyName}长度不能超过{MaxLength}。");
            RuleFor(u => u.UserName)
                .NotNull()
                .WithMessage("{PropertyName}是必须的。")
                .NotEmpty()
                .WithMessage("{PropertyName}是必须的。")
                .MaximumLength(AuthorityManagementConsts.MaxUserNameLength)
                .WithMessage("{PropertyName}长度不能超过{MaxLength}。");
            //RuleFor(u => u.EmailAddress)
            //    .NotNull()
            //    .WithMessage("{PropertyName}是必须的。")
            //    .NotEmpty()
            //    .WithMessage("{PropertyName}是必须的。")
            //    .EmailAddress()
            //    .WithMessage("请输入正确的{PropertyName}。")
            //    .MaximumLength(AuthorityManagementConsts.MaxEmailAddressLength)
            //    .WithMessage("{PropertyName}长度不能超过{MaxLength}。");
            //RuleFor(u => u.Password)
            //    .MaximumLength(AuthorityManagementConsts.MaxPasswordLength)
            //    .WithMessage("{PropertyName}长度不能超过{MaxLength}。");
            RuleFor(u => u.PhoneNumber)
                .MaximumLength(AuthorityManagementConsts.MaxPhoneNumberLength)
                .WithMessage("{PropertyName}长度不能超过{MaxLength}。")
                .Matches("^(((\\+\\d{2}-)?0\\d{2,3}-\\d{7,8})|((\\+\\d{2}-)?(\\d{2,3}-)?([1][3,4,5,7,8][0-9]\\d{8})))$")
                .WithMessage("请输入正确的{PropertyName}。");
        }
    }
}