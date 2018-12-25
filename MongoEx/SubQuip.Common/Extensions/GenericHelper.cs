using SubQuip.Common.CommonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;

namespace SubQuip.Common.Extensions
{
    public static class GenericHelper
    {
        public static DateTime CurrentDate { get => DateTime.Now; }

        public static UserClaim GetUserClaimDetails(ClaimsIdentity identity)
        {
            UserClaim userClaim = null;
            if (identity != null)
            {
                if (identity.Claims.Any())
                {
                    IEnumerable<Claim> claims = identity.Claims;

                    userClaim = new UserClaim
                    {
                        Name = identity.FindFirst("Name").Value                        
                    };
                    var emailClaim = identity.FindFirst(ClaimTypes.Email);
                    if (emailClaim != null)
                    {
                        userClaim.Email = emailClaim.Value;
                    }
                }
            }
            return userClaim;
        }

        public static string GetActiveUserId(this ClaimsIdentity identity)
        {
            var emailClaim = identity.FindFirst(ClaimTypes.Email);
            if (emailClaim != null)
            {
                return emailClaim.Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Maps the audit columns.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="identity"></param>
        public static void MapAuditColumns(this object model, ClaimsIdentity identity)
        {
            if (identity != null)
            {               
                var authorizedInfo = GenericHelper.GetUserClaimDetails(identity);
                if (model != null && authorizedInfo != null)
                {
                    if (Convert.ToDateTime(GetColumnValue(Constants.CreatedDate, model)) == default(DateTime))
                    {
                        SetColumnValue(Constants.IsActiveColumn, model, true);
                        SetColumnValue(Constants.CreatedDate, model, CurrentDate);
                        SetColumnValue(Constants.CreatedBy, model, authorizedInfo.Email);
                    }
                    SetColumnValue(Constants.ModifiedDate, model, CurrentDate);
                    SetColumnValue(Constants.ModifiedBy, model, authorizedInfo.Email);
                }
            }
        }


        /// <summary>
        /// Gets the column value.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static object GetColumnValue(String columnName, object entity)
        {
            var pinfo = entity.GetType().GetProperty(columnName);
            if (pinfo == null) { return null; }
            return pinfo.GetValue(entity, null);
        }

        /// <summary>
        /// Sets the column value.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="value">The value.</param>
        public static void SetColumnValue(String columnName, object entity, object value)
        {
            var pinfo = entity.GetType().GetProperty(columnName);
            if (pinfo != null) { pinfo.SetValue(entity, value, null); }
        }

    }
}
