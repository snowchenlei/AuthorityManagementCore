using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using JetBrains.Annotations;

namespace Anc.Users
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }

        [CanBeNull]
        int? Id { get; }

        [CanBeNull]
        string UserName { get; }

        [CanBeNull]
        string PhoneNumber { get; }

        bool PhoneNumberVerified { get; }

        [CanBeNull]
        string Email { get; }

        bool EmailVerified { get; }

        [NotNull]
        string[] Roles { get; }

        [CanBeNull]
        Claim FindClaim(string claimType);

        [NotNull]
        Claim[] FindClaims(string claimType);

        [NotNull]
        Claim[] GetAllClaims();

        bool IsInRole(string roleName);
    }
}