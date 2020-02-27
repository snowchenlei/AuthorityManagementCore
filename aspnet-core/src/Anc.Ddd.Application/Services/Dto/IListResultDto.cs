using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Application.Services.Dto
{
    /// <summary>
    /// This interface is defined to standardize to return a list of items to clients.
    /// </summary>
    /// <typeparam name="T">Type of the items in the list</typeparam>
    public interface IListResultDto<T>
    {
        /// <summary>
        /// List of items.
        /// </summary>
        IReadOnlyList<T> Items { get; set; }
    }
}